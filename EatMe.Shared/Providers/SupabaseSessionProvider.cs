using EatMe.Shared.Interfaces;
using Newtonsoft.Json;
using Supabase.Gotrue;
using Supabase.Gotrue.Interfaces;
/*
 * Sourced:
 * https://github.com/acupofjose/supasharp-todo/blob/master/SupasharpTodo.Shared/Providers/SupabaseSessionProvider.cs
 */

namespace EatMe.Shared.Providers {
    /// <summary>
    /// This Session Provider supplies a strategy for caching/destroying a session provided by Gotrue, locally for the user.
    /// </summary>
    public class SupabaseSessionProvider : IGotrueSessionPersistence<Session> {
        private const string CacheKey = ".gotrue.cache";
        private readonly ILocalStorageProvider _storageProvider;

        public SupabaseSessionProvider(ILocalStorageProvider storageProvider) =>
            _storageProvider = storageProvider;

        public void DestroySession() => _storageProvider.RemoveItem(CacheKey);

        public void SaveSession(Session session) {
            try {
                var serialized = JsonConvert.SerializeObject(session);
                _storageProvider.SetItem(CacheKey, serialized);
            } catch (Exception ex) {
                Console.WriteLine($"Failed to save session with:\n\t{ex}");
            }
        }

        public Session? LoadSession() {
            try {
                var json = _storageProvider.GetItem(CacheKey);
                if (string.IsNullOrEmpty(json)) return null;

                var session = JsonConvert.DeserializeObject<Session>(json);

                if (session != null && !session.Expired()) return session;

            } catch (Exception ex) {
                Console.WriteLine($"Failed to load session with:\n\t{ex}");
            }
            return null;
        }
    }
}
