using CommunityToolkit.Mvvm.ComponentModel;
using QuranParser;
using System.Collections.ObjectModel;

namespace AlTanzeel.ViewModel
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        ObservableCollection<Sura> suras;

        [ObservableProperty]
        DateTime date;

        [ObservableProperty]
        Sura selectedSura;

        public MainViewModel()
        {
            //Suras = new ObservableCollection<Sura>();
            Date = DateTime.Now;
            LoadAndDisplayQuranAsync();
        }

        private async void LoadAndDisplayQuranAsync()
        {
            QuranXmlParser parser = new();
            var surasList = await parser.ParseQuranXmlAsync();
            Suras = new ObservableCollection<Sura>(surasList);
            SelectedSura = Suras.FirstOrDefault();
        }
    }
}
