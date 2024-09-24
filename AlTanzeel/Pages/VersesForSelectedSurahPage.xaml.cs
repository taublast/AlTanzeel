using AlTanzeel.ViewModel;

namespace AlTanzeel.Pages;

public partial class VersesForSelectedSurahPage : ContentPage
{
    public VersesForSelectedSurahPage(MainViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
        this.Title = $"Verses For {vm.SelectedSura.Name}";
    }

    private void CollectionView_Scrolled(object sender, ItemsViewScrolledEventArgs e)
    {
        if(sender is SearchBar)
        {
            return;
        }
        searchBar.Unfocus();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        // Access the ViewModel and clear the SearchQuery value
        if (BindingContext is MainViewModel vm)
        {
            vm.SearchQuery = string.Empty;
            vm.SearchQueryMode = QuranParser.SearchQueryType.Surah;
        }
    }
}
