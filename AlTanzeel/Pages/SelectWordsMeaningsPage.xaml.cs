using AlTanzeel.ViewModel;
using QuranParser;

namespace AlTanzeel.Pages;

public partial class SelectWordsMeaningsPage : ContentPage
{
    public SelectWordsMeaningsPage(MainViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is MainViewModel vm)
        {
            selectVersesLabel.Text = vm.SelectedAyasForWordsMeanings.Count > 0
                ? "Tap here to select more verses."
                : "Please tap here to select verses.";
            if (vm.SelectedAyasForWordsMeanings.Count > 0)
            {
                var word = vm.WordsForWordsMeaning.Count > 1 ? "words" : "word";
                var selectedWordsCount = vm.WordsForWordsMeaning.Count(word => word.IsSelected);
                collectionViewHeaderText.Text = $"You have selected {selectedWordsCount} {word} for words meaning.";
            }
            else
            {
                collectionViewHeaderText.Text = string.Empty;
            }
        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        if (BindingContext is MainViewModel vm)
        {
            vm.SearchQuery = string.Empty;
            vm.SearchQueryMode = SearchQueryType.Surah;
        }
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        if (BindingContext is MainViewModel vm)
        {
            selectVersesLabel.Text = vm.SelectedAyasForWordsMeanings.Count > 0
                ? "Tap here to select more verses."
                : "Please tap here to select verses.";
            if (vm.SelectedAyasForWordsMeanings.Count > 0)
            {
                var word = vm.WordsForWordsMeaning.Count > 1 ? "words" : "word";
                var selectedWordsCount = vm.WordsForWordsMeaning.Count(word => word.IsSelected);
                collectionViewHeaderText.Text = $"You have selected {selectedWordsCount} {word} for words meaning";
            }
            else
            {
                collectionViewHeaderText.Text = string.Empty;
            }
        }
    }
}