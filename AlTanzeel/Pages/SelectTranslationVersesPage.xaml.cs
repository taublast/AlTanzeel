using AlTanzeel.ViewModel;

namespace AlTanzeel.Pages;

public partial class SelectTranslationVersesPage : ContentPage
{
    public SelectTranslationVersesPage(MainViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is MainViewModel vm)
        {
            selectVersesLabel.Text = vm.SelectedAyasForTranslation.Count > 0
                ? "Tap here to select more verses."
                : "Please tap here to select verses.";
            if (vm.SelectedAyasForTranslation.Count > 0)
            {
                var word = vm.SelectedAyasForTranslation.Count > 1 ? "Ayas" : "Aya";
                collectionViewHeaderText.Text = $"You selected {vm.SelectedAyasForTranslation.Count} {word}";
            }
            else
            {
                collectionViewHeaderText.Text = string.Empty;
            }
        }
    }
}