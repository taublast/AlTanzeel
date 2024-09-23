namespace AlTanzeel
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(SelectSurahPage), typeof(SelectSurahPage));
        }
    }
}
