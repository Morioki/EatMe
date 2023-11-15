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

        ///<summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        public static void AddSupaSharedCore(this IServiceCollection services) {
            // Register Supabase with its session provider
            services.AddScoped(provider => {
                const string url = "";
                const string publicKey = "";

                var localStorageProvider = provider.GetRequiredService<ILocalStorageProvider>();
                return new Client(url, publicKey, new SupabaseOptions {
                    AutoRefreshToken = true,
                    AutoConnectRealtime = true,
                    SessionHandler = new SupabaseSessionProvider(localStorageProvider!)
                });
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

            Console.WriteLine("Initialized Supabase Core.");
        }
    }
}
