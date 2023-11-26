using EatMe.Shared.Interfaces;

namespace EatMe.Shared.Services {
    public class LocalStorageProvider : ILocalStorageProvider {
        private ISecureStorage Instance => SecureStorage.Default;

        public string GetItem(string key) => Instance.GetAsync(key).Result;

        public void RemoveItem(string key) => Instance.Remove(key); // TODO Look to making bool?

        public void SetItem(string key, string value) => Instance.SetAsync(key, value);

        public void Empty() => Instance.RemoveAll();
    }
}
