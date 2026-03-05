using Maui2.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace Maui2;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        var assembly = Assembly.GetExecutingAssembly();

        using var stream = assembly.GetManifestResourceStream("Maui2.appsettings.json");

        if (stream != null)
        {
            var config = new ConfigurationBuilder()
                .AddJsonStream(stream)
                .Build();

            builder.Configuration.AddConfiguration(config);
        }

        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        builder.Services.AddMauiBlazorWebView();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        builder.Services.AddSingleton<ConfigurationService>();
        builder.Services.AddSingleton<RabbitMqService>();
        builder.Services.AddSingleton<TranslatorService>();

        return builder.Build();
    }
}