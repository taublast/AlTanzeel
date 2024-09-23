using AlTanzeel.ViewModel;

namespace AlTanzeel.Pages;

public partial class VersesForSelectedSurahPage : ContentPage
{
	public VersesForSelectedSurahPage(MainViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}