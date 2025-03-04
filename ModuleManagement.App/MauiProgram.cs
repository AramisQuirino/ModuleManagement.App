using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace ModuleManagement.App;

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
            });

        builder.Services.AddMauiBlazorWebView();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        // Configurar HttpClient para conectar a la API
        // Para Android, recuerda que "localhost" se debe reemplazar por "10.0.2.2"
#if ANDROID
        builder.Services.AddHttpClient("API", client =>
        {
            client.BaseAddress = new Uri("https://10.0.2.2:7290/");
        });
#else
        builder.Services.AddHttpClient("API", client =>
        {
            client.BaseAddress = new Uri("https://localhost:7290/");
        });
#endif

        // Registra el HttpClient inyectado, de modo que puedas usarlo directamente en tus componentes o servicios.
        builder.Services.AddScoped(sp =>
            sp.GetRequiredService<IHttpClientFactory>().CreateClient("API"));

        return builder.Build();
    }
}
