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
        ObservableCollection<Surah> suras;

        [ObservableProperty]
        DateTime date;

        [ObservableProperty]
        Surah selectedSura;

        [ObservableProperty]
        string searchQuery;

        [ObservableProperty]
        ObservableCollection<Surah> filteredSuras;

        public MainViewModel()
        {
            Date = DateTime.Now;
            LoadAndDisplayQuranAsync();
            SearchQuery = string.Empty;
        }

        private async void LoadAndDisplayQuranAsync()
        {
            QuranXmlParser parser = new();
            var surasList = await parser.ParseQuranXmlAsync();
            Suras = new ObservableCollection<Surah>(surasList);
            FilteredSuras = new ObservableCollection<Surah>(surasList);
            SelectedSura = Suras.FirstOrDefault();
        }

        [RelayCommand]
        async Task NavigateToSelectSuraPage()
        {
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
            await AppShell.Current.GoToAsync($"{nameof(VersesForSelectedSurahPage)}");
        }

        [RelayCommand]
        async void SelectSurah(Surah surah)
        {
            this.SelectedSura = surah;
            await Shell.Current.GoToAsync("..");

        }

        partial void OnSearchQueryChanged(string value)
        {
            FilterSurahs();
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
