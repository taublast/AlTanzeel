using AlTanzeel.Pages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QuranParser;
using System.Collections.ObjectModel;

namespace AlTanzeel.ViewModel
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        DateTime date;

        [ObservableProperty]
        SearchQueryType searchQueryMode;

        [ObservableProperty]
        TranslationVersesDataSetType translationVersesDataSetType;

        [ObservableProperty]
        string searchQuery;

        [ObservableProperty]
        ObservableCollection<Surah> suras;

        [ObservableProperty]
        ObservableCollection<Surah> filteredSuras;

        [ObservableProperty]
        Surah selectedSura;

        [ObservableProperty]
        ObservableCollection<Aya> ayasOfSelectedSurah;

        [ObservableProperty]
        ObservableCollection<Aya> filteredAyasOfSelectedSurahForTranslation;

        [ObservableProperty]
        ObservableCollection<Aya> filteredAyasOfSelectedSurahForWordsMeaning;

        [ObservableProperty]
        ObservableCollection<Aya> selectedAyasForTranslation;

        [ObservableProperty]
        ObservableCollection<Aya> selectedAyasForWordsMeanings;

        [ObservableProperty]
        ObservableCollection<WordForWordsMeaning> wordsForWordsMeaning;

        public MainViewModel()
        {
            Suras = [];
            FilteredSuras = [];
            SelectedSura = new();
            SearchQueryMode = SearchQueryType.Surah;
            translationVersesDataSetType = TranslationVersesDataSetType.Verse;
            Date = DateTime.Now;
            LoadAndDisplayQuranAsync();
            SearchQuery = string.Empty;
            AyasOfSelectedSurah = [];
            FilteredAyasOfSelectedSurahForTranslation = [];
            FilteredAyasOfSelectedSurahForWordsMeaning = [];
            SelectedAyasForTranslation = [];
            SelectedAyasForWordsMeanings = [];
            WordsForWordsMeaning = [];
        }

        private async void LoadAndDisplayQuranAsync()
        {
            QuranXmlParser parser = new();
            var surasList = await parser.ParseQuranXmlAsync();
            Suras = new ObservableCollection<Surah>(surasList);
            FilteredSuras = new ObservableCollection<Surah>(surasList);
#pragma warning disable CS8601 // Possible null reference assignment.
            SelectedSura = Suras.FirstOrDefault();
#pragma warning restore CS8601 // Possible null reference assignment.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            AyasOfSelectedSurah = new ObservableCollection<Aya>(SelectedSura.Ayas);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            FilteredAyasOfSelectedSurahForTranslation = new ObservableCollection<Aya>(SelectedSura.Ayas);
            FilteredAyasOfSelectedSurahForWordsMeaning = new ObservableCollection<Aya>(
                SelectedSura.Ayas.Select(aya => new Aya
                {
                    Index = aya.Index,
                    Text = aya.Text,
                    Bismillah = aya.Bismillah,
                    IsSelected = false
                }).ToList()
            );
        }

        [RelayCommand]
        public void SelectAya(Aya aya)
        {
            // Toggle the IsSelected property of the Aya
            aya.IsSelected = !aya.IsSelected;

            if (TranslationVersesDataSetType == TranslationVersesDataSetType.Verse)
            {
                // Update the list of selected Ayas
                if (aya.IsSelected)
                {
                    if (!SelectedAyasForTranslation.Contains(aya))
                        SelectedAyasForTranslation.Add(aya);
                }
                else
                {
                    _ = SelectedAyasForTranslation.Remove(aya);
                }
            }
            else
            {
                // Update the list of selected Ayas
                if (aya.IsSelected)
                {
                    if (!SelectedAyasForWordsMeanings.Contains(aya))
                        SelectedAyasForWordsMeanings.Add(aya);
                }
                else
                {
                    _ = SelectedAyasForWordsMeanings.Remove(aya);
                }
                WordsForWordsMeaning = new ObservableCollection<WordForWordsMeaning>(
                    SelectedAyasForWordsMeanings
                        .SelectMany(aya => aya.Text.Split(' '))
                        .Where(word => !string.IsNullOrWhiteSpace(word) && !IsOnlyDiacritic(word)) // Exclude words that are only diacritical marks
                        .Distinct()
                        .Select((word, index) => new WordForWordsMeaning
                        {
                            Id = index,
                            Word = word,
                            IsSelected = false
                        })
                );
            }
        }

        // Helper method to check if a word consists entirely of diacritical marks
        private bool IsOnlyDiacritic(string word)
        {
            // If every character in the word is a diacritical mark, we return true, otherwise false.
            return word.All(c =>
                (c >= '\u0610' && c <= '\u061A') ||  // Arabic Extended-A
                (c >= '\u064B' && c <= '\u065F') ||  // Arabic diacritics
                (c >= '\u06D6' && c <= '\u06DC') ||  // Arabic small high ligatures
                (c >= '\u06DF' && c <= '\u06E8') ||  // More Arabic small high letters
                (c >= '\u06EA' && c <= '\u06ED') ||  // More ligatures
                (c >= '\u08D4' && c <= '\u08E1'));   // Quranic annotations
        }
        [RelayCommand]
        async Task NavigateToSelectSuraPage()
        {
            this.SearchQueryMode = SearchQueryType.Surah;
            await AppShell.Current.GoToAsync($"{nameof(SelectSurahPage)}");
        }

        [RelayCommand]
        async Task NavigateToSelectTranslationVersesPage()
        {
            TranslationVersesDataSetType = TranslationVersesDataSetType.Verse;
            await AppShell.Current.GoToAsync($"{nameof(SelectTranslationVersesPage)}");
        }

        [RelayCommand]
        async Task NavigateToSelectWordsMeaningsPage()
        {
            TranslationVersesDataSetType = TranslationVersesDataSetType.WordsMeanings;
            await AppShell.Current.GoToAsync($"{nameof(SelectWordsMeaningsPage)}");
        }

        [RelayCommand]
        async Task NavigateToSelectVerses()
        {
            this.SearchQueryMode = SearchQueryType.Aya;
            await AppShell.Current.GoToAsync($"{nameof(VersesForSelectedSurahPage)}");
        }

        [RelayCommand]
        async Task SelectSurah(Surah surah)
        {
            this.SelectedSura = surah;
            this.SearchQuery = string.Empty;
            AyasOfSelectedSurah = new ObservableCollection<Aya>(SelectedSura.Ayas);
            FilteredAyasOfSelectedSurahForTranslation = new ObservableCollection<Aya>(SelectedSura.Ayas);
            FilteredAyasOfSelectedSurahForWordsMeaning = new ObservableCollection<Aya>(SelectedSura.Ayas);
            await Shell.Current.GoToAsync("..");

        }

        [RelayCommand]
        void SelectWord(WordForWordsMeaning word)
        {
            word.IsSelected = !word.IsSelected;
        }

        partial void OnSearchQueryChanged(string value)
        {
            if (SearchQueryMode == SearchQueryType.Surah)
            {
                FilterSurahs();
            }
            else if (SearchQueryMode == SearchQueryType.Aya)
            {
                FilterAyas();
            }
        }
        private void FilterAyas()
        {
            if (string.IsNullOrWhiteSpace(SearchQuery))
            {
                FilteredAyasOfSelectedSurahForTranslation = new ObservableCollection<Aya>(SelectedSura.Ayas);
            }
            else
            {
                var filtered = SelectedSura.Ayas.Where(a => a.AyaWithIndex.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase)).ToList();
                FilteredAyasOfSelectedSurahForTranslation = new ObservableCollection<Aya>(filtered);
            }
        }
        private void FilterSurahs()
        {
            if (string.IsNullOrWhiteSpace(SearchQuery))
            {
                // If no search query, display all surahs
                FilteredSuras = Suras;
            }
            else
            {
                // Filter surahs based on the search query
                var filtered = Suras.Where(s => s.SurahInfo.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase)).ToList();
                FilteredSuras = new ObservableCollection<Surah>(filtered);
            }

            // Notify that FilteredSuras has changed
            OnPropertyChanged(nameof(FilteredSuras));
        }
    }
}
