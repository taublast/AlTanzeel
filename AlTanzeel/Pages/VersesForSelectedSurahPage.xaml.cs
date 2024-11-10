using AlTanzeel.ViewModel;
using QuranParser;

namespace AlTanzeel.Pages;

public partial class VersesForSelectedSurahPage : ContentPage
{
    public VersesForSelectedSurahPage(MainViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
        Title = $"Verses For {vm.SelectedSura.Name}";
    }

    private void CollectionView_Scrolled(object sender, ItemsViewScrolledEventArgs e)
    {
        searchBar.Unfocus();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is MainViewModel vm)
        {
            if (vm.TranslationVersesDataSetType == TranslationVersesDataSetType.Verse)
                collectionView.ItemsSource = vm.FilteredAyasOfSelectedSurahForTranslation;
            else
                collectionView.ItemsSource = vm.FilteredAyasOfSelectedSurahForWordsMeaning;
        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        // Access the ViewModel and clear the SearchQuery value
        if (BindingContext is MainViewModel vm)
        {
            vm.SearchQuery = string.Empty;
            vm.SearchQueryMode = SearchQueryType.Surah;
        }
    }
}