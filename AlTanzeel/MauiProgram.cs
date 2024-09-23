using AlTanzeel.Pages;
using AlTanzeel.ViewModel;
using Microsoft.Extensions.Logging;

namespace AlTanzeel
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<SelectSurahPage>();
            builder.Services.AddSingleton<MainViewModel>();
            builder.Services.AddSingleton<SelectTranslationVersesPage>();
            builder.Services.AddSingleton<VersesForSelectedSurahPage>();
            return builder.Build();
        }
    }
}
