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
        ObservableCollection<Aya> filteredAyasOfSelectedSurah;

        public MainViewModel()
        {
            SearchQueryMode = SearchQueryType.Surah;
            Date = DateTime.Now;
            LoadAndDisplayQuranAsync();
            SearchQuery = string.Empty;
            AyasOfSelectedSurah = new ObservableCollection<Aya>();
            FilteredAyasOfSelectedSurah = new ObservableCollection<Aya>();
        }

        private async void LoadAndDisplayQuranAsync()
        {
            QuranXmlParser parser = new();
            var surasList = await parser.ParseQuranXmlAsync();
            Suras = new ObservableCollection<Surah>(surasList);
            FilteredSuras = new ObservableCollection<Surah>(surasList);
            SelectedSura = Suras.FirstOrDefault();
            AyasOfSelectedSurah = new ObservableCollection<Aya>(SelectedSura.Ayas);
            FilteredAyasOfSelectedSurah = new ObservableCollection<Aya>(SelectedSura.Ayas);
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
            await AppShell.Current.GoToAsync($"{nameof(SelectTranslationVersesPage)}");
        }

        [RelayCommand]
        async Task NavigateToSelectVersesForTranslationPage()
        {
            this.SearchQueryMode = SearchQueryType.Aya;
            await AppShell.Current.GoToAsync($"{nameof(VersesForSelectedSurahPage)}");
        }

        [RelayCommand]
        async void SelectSurah(Surah surah)
        {
            this.SelectedSura = surah;
            this.SearchQuery = string.Empty;
            AyasOfSelectedSurah = new ObservableCollection<Aya>(SelectedSura.Ayas);
            FilteredAyasOfSelectedSurah = new ObservableCollection<Aya>(SelectedSura.Ayas);
            await Shell.Current.GoToAsync("..");

        }

        partial void OnSearchQueryChanged(string value)
        {
            if (SearchQueryMode == SearchQueryType.Surah)
            {
                FilterSurahs();
            }
            else if(SearchQueryMode == SearchQueryType.Aya)
            {
                FilterAyas();
            }
        }
        private void FilterAyas()
        {
            if(string.IsNullOrWhiteSpace(SearchQuery))
            {
                FilteredAyasOfSelectedSurah = new ObservableCollection<Aya>(SelectedSura.Ayas);
            }
            else
            {
                var filtered = SelectedSura.Ayas.Where(a => a.AyaWithIndex.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase)).ToList();
                FilteredAyasOfSelectedSurah = new ObservableCollection<Aya>(filtered);
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
