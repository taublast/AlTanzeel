using AlTanzeel.Pages;

namespace AlTanzeel;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute(nameof(SelectSurahPage), typeof(SelectSurahPage));
        Routing.RegisterRoute(nameof(SelectTranslationVersesPage), typeof(SelectTranslationVersesPage));
        Routing.RegisterRoute(nameof(VersesForSelectedSurahPage), typeof(VersesForSelectedSurahPage));
        Routing.RegisterRoute(nameof(SelectWordsMeaningsPage), typeof(SelectWordsMeaningsPage));
    }
}