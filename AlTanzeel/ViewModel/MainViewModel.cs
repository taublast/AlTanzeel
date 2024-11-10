using System.Collections.ObjectModel;
using AlTanzeel.Pages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QuranParser;

namespace AlTanzeel.ViewModel;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty] private ObservableCollection<Aya> ayasOfSelectedSurah;

    [ObservableProperty] private DateTime date;

    [ObservableProperty] private ObservableCollection<Aya> filteredAyasOfSelectedSurahForTranslation;

    [ObservableProperty] private ObservableCollection<Aya> filteredAyasOfSelectedSurahForWordsMeaning;

    [ObservableProperty] private ObservableCollection<Surah> filteredSuras;

    [ObservableProperty] private string searchQuery;

    [ObservableProperty] private SearchQueryType searchQueryMode;

    [ObservableProperty] private ObservableCollection<Aya> selectedAyasForTranslation;

    [ObservableProperty] private ObservableCollection<Aya> selectedAyasForWordsMeanings;

    [ObservableProperty] private Surah selectedSura;

    [ObservableProperty] private ObservableCollection<Surah> suras;

    [ObservableProperty] private TranslationVersesDataSetType translationVersesDataSetType;

    [ObservableProperty] private ObservableCollection<WordForWordsMeaning> wordsForWordsMeaning;

    public MainViewModel()
    {
        Suras = [];
        FilteredSuras = [];
        SelectedSura = new Surah();
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
                    .Where(word =>
                        !string.IsNullOrWhiteSpace(word) &&
                        !IsOnlyDiacritic(word)) // Exclude words that are only diacritical marks
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
            (c >= '\u0610' && c <= '\u061A') || // Arabic Extended-A
            (c >= '\u064B' && c <= '\u065F') || // Arabic diacritics
            (c >= '\u06D6' && c <= '\u06DC') || // Arabic small high ligatures
            (c >= '\u06DF' && c <= '\u06E8') || // More Arabic small high letters
            (c >= '\u06EA' && c <= '\u06ED') || // More ligatures
            (c >= '\u08D4' && c <= '\u08E1')); // Quranic annotations
    }

    [RelayCommand]
    private async Task NavigateToSelectSuraPage()
    {
        SearchQueryMode = SearchQueryType.Surah;
        await Shell.Current.GoToAsync($"{nameof(SelectSurahPage)}");
    }

    [RelayCommand]
    private async Task NavigateToSelectTranslationVersesPage()
    {
        TranslationVersesDataSetType = TranslationVersesDataSetType.Verse;
        await Shell.Current.GoToAsync($"{nameof(SelectTranslationVersesPage)}");
    }

    [RelayCommand]
    private async Task NavigateToSelectWordsMeaningsPage()
    {
        TranslationVersesDataSetType = TranslationVersesDataSetType.WordsMeanings;
        await Shell.Current.GoToAsync($"{nameof(SelectWordsMeaningsPage)}");
    }

    [RelayCommand]
    private async Task NavigateToSelectVerses()
    {
        SearchQueryMode = SearchQueryType.Aya;
        await Shell.Current.GoToAsync($"{nameof(VersesForSelectedSurahPage)}");
    }

    [RelayCommand]
    private async Task SelectSurah(Surah surah)
    {
        SelectedSura = surah;
        SearchQuery = string.Empty;
        AyasOfSelectedSurah = new ObservableCollection<Aya>(SelectedSura.Ayas);
        FilteredAyasOfSelectedSurahForTranslation = new ObservableCollection<Aya>(SelectedSura.Ayas);
        FilteredAyasOfSelectedSurahForWordsMeaning = new ObservableCollection<Aya>(SelectedSura.Ayas);
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    private void SelectWord(WordForWordsMeaning word)
    {
        word.IsSelected = !word.IsSelected;
    }

    partial void OnSearchQueryChanged(string value)
    {
        if (SearchQueryMode == SearchQueryType.Surah)
            FilterSurahs();
        else if (SearchQueryMode == SearchQueryType.Aya) FilterAyas();
    }

    private void FilterAyas()
    {
        if (string.IsNullOrWhiteSpace(SearchQuery))
        {
            FilteredAyasOfSelectedSurahForTranslation = new ObservableCollection<Aya>(SelectedSura.Ayas);
        }
        else
        {
            var filtered = SelectedSura.Ayas
                .Where(a => a.AyaWithIndex.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase)).ToList();
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
            var filtered = Suras.Where(s => s.SurahInfo.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase))
                .ToList();
            FilteredSuras = new ObservableCollection<Surah>(filtered);
        }

        // Notify that FilteredSuras has changed
        OnPropertyChanged(nameof(FilteredSuras));
    }
}