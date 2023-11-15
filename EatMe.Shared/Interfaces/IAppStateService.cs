/* 
 * Sourced:
 * https://github.com/acupofjose/supasharp-todo/blob/master/SupasharpTodo.Shared/Interfaces/IAppStateService.cs
 */

using Supabase.Gotrue;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace EatMe.Shared.Interfaces {
    public interface IAppStateService : INotifyPropertyChanged {
        #region Properties

        ObservableCollection<string> Errors { get; }
        bool IsLoading { get; }
        User User { get; }
        bool IsLoggedIn { get; }
        string? AvatarUrl { get; }

        #endregion

        #region App Focus Change

        // TODO Update section
        enum AppSection {
            Section1,
            Section2,
            Section3
        }

        AppSection FocusedSection { get; }

        delegate void FocusedSectionChanged(AppSection lostFocus, AppSection gainedFocus);

        void AddFocusedSectionChangedListener(FocusedSectionChanged listener);
        void RemoveFocusedSectionChangedListener(FocusedSectionChanged listener);
        void ClearFocusedSectionChangedListeners(FocusedSectionChanged listener);
        void SetFocusedSection(AppSection gainedFocus);

        #endregion
    }
}
