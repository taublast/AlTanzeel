﻿using AlTanzeel.Pages;
using AlTanzeel.ViewModel;
using DrawnUi.Maui.Draw;
using Microsoft.Extensions.Logging;

namespace AlTanzeel;

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
                fonts.AddFont("pdms-saleem-quranfont.ttf", "PDMS-Saleem");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif
        builder.UseDrawnUi();
        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddSingleton<SelectSurahPage>();
        builder.Services.AddSingleton<MainViewModel>();
        builder.Services.AddSingleton<SelectTranslationVersesPage>();
        builder.Services.AddSingleton<VersesForSelectedSurahPage>();
        builder.Services.AddSingleton<SelectWordsMeaningsPage>();
        return builder.Build();
    }
}