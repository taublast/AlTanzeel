using AlTanzeel.ViewModel;
using QuranParser;

namespace AlTanzeel
{
    public partial class MainPage : ContentPage
    {

        public MainPage(MainViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }

        private void QuizDatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {

        }
    }
}
