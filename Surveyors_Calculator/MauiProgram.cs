using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using CommunityToolkit.Maui;
using Surveyors_Calculator.View;
//using Surveyors_Calculator.ViewModel;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Surveyors_Calculator.Services;

namespace Surveyors_Calculator;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        SQLitePCL.Batteries_V2.Init();
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services.AddSingleton<SqliteConnectionFactory>();
        //builder.Services.AddSingleton<IPopupService, Surveyors_Calculator.Services.PopupService>();
        //builder.Services.AddSingleton<FileDBService>();

        builder.Services.AddSingleton<Formulae>();
        builder.Services.AddSingleton<Coordinate>();


        builder.Services.AddSingleton<CalculationsMenuPageViewModel>();
        builder.Services.AddTransient<CalculationsPageViewModel>();
        builder.Services.AddTransient<MainPageViewModel>();
        builder.Services.AddTransient<InputViewModel>();
        builder.Services.AddSingleton<QuickTransformViewModel>();
        builder.Services.AddSingleton<ConversionsViewModel>();
        builder.Services.AddSingleton<SettingsViewModel>();
        builder.Services.AddTransient<RecentViewModel>();
        builder.Services.AddTransient<AboutViewModel>();

        builder.Services.AddSingleton<CalculationsMenuPage>();
        builder.Services.AddTransient<CalculationsPage>();
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<Input>();
        builder.Services.AddSingleton<QuickTransform>();
        builder.Services.AddSingleton<Conversions>();
        builder.Services.AddSingleton<Settings>();
        builder.Services.AddTransient<Recent>();
        builder.Services.AddTransient<About>();


        return builder.Build();
    }
}
