/*
 * Sourced:
 * https://github.com/acupofjose/supasharp-todo/blob/master/SupasharpTodo.Shared/Extensions/ServiceCollectionExtensions.cs
 */

using EatMe.Shared.Interfaces;
using EatMe.Shared.Providers;
using EatMe.Shared.Services;
using Microsoft.Extensions.DependencyInjection;
using Postgrest.Interfaces;
using Supabase;

namespace EatMe.Shared.Extensions {
    public static class ServiceCollectionExtensions {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        public static void AddSupaSharedCore(this IServiceCollection services, string BaseAddress) {
            // Register Supabase with its session provider
            services.AddScoped(provider => {
                string url = BaseAddress; // TODO 
                const string publicKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZS1kZW1vIiwicm9sZSI6ImFub24iLCJleHAiOjE5ODM4MTI5OTZ9.CRXP1A7WOeoJeXxjNni43kdQwgnWNReilDMblYTn_I0"; // TODO

                var localStorageProvider = provider.GetRequiredService<ILocalStorageProvider>();
                var client = new Client(url, publicKey, new SupabaseOptions {
                    AutoRefreshToken = true,
                    AutoConnectRealtime = true,
                    SessionHandler = new SupabaseSessionProvider(localStorageProvider!)
                });

                client.Auth.AddDebugListener(LogDebug);

                return client;
            });

            // Register a postgrest cache provider
            services.AddScoped<IPostgrestCacheProvider, PostgrestCacheProvider>(p =>
                new PostgrestCacheProvider(p.GetRequiredService<ILocalStorageProvider>()));

            // Register State Handlers and Services
            services.AddScoped<IAppStateService>(p => new AppStateService(p.GetRequiredService<Client>()));
            //services.AddScoped<IRecipeService>(p =>
            //new RecipeService(p.GetRequiredService<IAppStateService>(),
            //    p.GetRequiredService<Client>(),
            //    p.GetRequiredService<IPostgrestCacheProvider>()));

            services.AddScoped<IIngredientService>(p =>
                new IngredientService(p.GetRequiredService<IAppStateService>(),
                p.GetRequiredService<Client>(),
                p.GetRequiredService<IPostgrestCacheProvider>()));

            Console.WriteLine("Initialized Supabase Core.");
        }

        private static void LogDebug(string arg1, Exception? exception) {
            Console.WriteLine(arg1 + ": " + exception);
        }
    }
}
