using AlTanzeel.ViewModel;
using Microsoft.Maui.Controls;

namespace AlTanzeel;

public partial class SelectSurahPage : ContentPage
{
	public SelectSurahPage(MainViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}

    private void CollectionView_Scrolled(object sender, ItemsViewScrolledEventArgs e)
    {
        // Dismiss the keyboard when the user scrolls
        // Check if the SearchBar is focused (i.e., the keyboard is open)
        if (searchBar.IsFocused)
        {
            // Unfocus the SearchBar to dismiss the keyboard
            searchBar.Unfocus();
        }
    }
}