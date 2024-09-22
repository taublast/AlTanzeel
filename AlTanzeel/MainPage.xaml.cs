using QuranParser;

namespace AlTanzeel
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadAndDisplayQuran();
        }

        public async Task LoadAndDisplayQuran()
        {
            QuranXmlParser parser = new QuranXmlParser();
            List<Sura> suras = await parser.ParseQuranXmlAsync();

            // Display all Surahs
            foreach (var sura in suras)
            {
                Console.WriteLine($"Sura: {sura.Name} (Index: {sura.Index})");
            }
        }

        private void QuizDatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {

        }
    }
}
