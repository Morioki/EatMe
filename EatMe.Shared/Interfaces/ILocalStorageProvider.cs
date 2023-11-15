/*
 * Sourced: 
 * https://github.com/acupofjose/supasharp-todo/blob/master/SupasharpTodo.Shared/Interfaces/ILocalStorageProvider.cs
 */
namespace EatMe.Shared.Interfaces {
    public interface ILocalStorageProvider {
        string GetItem(string key);
        void RemoveItem(string key);
        void SetItem(string key, string value);

        void Empty();
    }
}
