using EatMe.Shared.Extensions;
using EatMe.Shared.Interfaces;
using EatMe.Shared.Services;
using Microsoft.Extensions.Logging;

namespace EatMe.Client {
    public static class MauiProgram {
        public static MauiApp CreateMauiApp() {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts => {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            //return builder.Build();
            string BaseAddress =
                DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2:40001" : "http://localhost:40001";
            builder.Services.AddScoped<ILocalStorageProvider, LocalStorageProvider>();
            builder.Services.AddSupaSharedCore(BaseAddress);

            var app = builder.Build();

            // TODO
            MainThread.BeginInvokeOnMainThread(async () => {
                var supabase = app.Services.GetRequiredService<Supabase.Client>();
                await supabase.InitializeAsync();
                supabase.Auth.LoadSession(); // Causes system hang
            });

            return app;
        }
    }
}
