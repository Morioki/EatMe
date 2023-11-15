/*
 * Sourced:
 * https://github.com/acupofjose/supasharp-todo/blob/master/SupasharpTodo.Shared/Providers/PostgrestCacheProvider.cs
 */

using EatMe.Shared.Interfaces;
using Newtonsoft.Json;

namespace EatMe.Shared.Providers {
    /// <summary>
    /// This cache provider handles storing a particular query (identified by its url) into a local caching strategy.
    /// </summary>
    public class PostgrestCacheProvider : Postgrest.Interfaces.IPostgrestCacheProvider {
        private readonly ILocalStorageProvider _storageProvider;

        public PostgrestCacheProvider(ILocalStorageProvider storageProvider) =>
            _storageProvider = storageProvider;

        public Task<T?> GetItem<T>(string key) {
            var stored = _storageProvider.GetItem(key);

            if (string.IsNullOrEmpty(stored)) return Task.FromResult<T?>(default);

            var deserialized = JsonConvert.DeserializeObject<T>(stored);

            if (deserialized == null) return Task.FromResult<T?>(default);

            return Task.FromResult(deserialized);
        }

        public Task SetItem(string key, object value) {
            var serialized = JsonConvert.SerializeObject(value);
            _storageProvider.SetItem(key, serialized);
            return Task.CompletedTask;
        }

        public Task ClearItem(string key) {
            _storageProvider.RemoveItem(key);
            return Task.CompletedTask;
        }

        public Task Empty() {
            _storageProvider.Empty();
            return Task.CompletedTask;
        }
    }
}
