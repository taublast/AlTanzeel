using AlTanzeel.ViewModel;

namespace AlTanzeel.Pages;

public partial class SelectTranslationVersesPage : ContentPage
{
	public SelectTranslationVersesPage(MainViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}