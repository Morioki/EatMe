/*
 * Sourced:
 * https://github.com/acupofjose/supasharp-todo/blob/master/SupasharpTodo.Shared/Services/AppStateService.cs
 */

using EatMe.Shared.Interfaces;
using Supabase.Gotrue;
using Supabase.Gotrue.Interfaces;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EatMe.Shared.Services {
    /// <summary>
    /// Represents the global app state
    /// </summary>
    public sealed class AppStateService : IAppStateService {
        /// <inheritdoc />
        public event PropertyChangedEventHandler? PropertyChanged;

        private Supabase.Client Supabase { get; }

        /// <summary>
        /// Sets the Focused App Section to scope keyboard shortcuts appropriately.
        /// </summary>
        public IAppStateService.AppSection FocusedSection { get; private set; } =
            IAppStateService.AppSection.Section1; // TODO Update Section

        private readonly List<IAppStateService.FocusedSectionChanged> _focusedSectionChangedListeners = new();

        /// <summary>
        /// A local state of errors that can be displayed to user
        /// </summary>
        public ObservableCollection<string> Errors { get; } = new();

        public bool _isLoading;

        /// <summary>
        /// If the global app is in a loading state.
        /// </summary>
        public bool IsLoading {
            get => _isLoading;
            set => SetField(ref _isLoading, value);
        }

        private bool _isLoggedIn;

        /// <summary>
        /// If user is logged in.
        /// </summary>
        public bool IsLoggedIn {
            get => _isLoggedIn;
            private set => SetField(ref _isLoggedIn, value);
        }

        /// <summary>
        /// Shorthand accessor for the current user.
        /// </summary>
        public User? User => Supabase.Auth.CurrentUser;

        /// <summary>
        /// Returns the avatar URl of the current user 
        /// </summary>
        public string? AvatarUrl {
            get {
                if (User != null && User.UserMetadata.TryGetValue("avatar_url", out var avatarUrl)) {
                    return avatarUrl.ToString();
                }
                return null;
            }
        }

        /// <summary>
        /// Registers the App State Service.
        /// </summary>
        /// <param name="client"></param>
        public AppStateService(Supabase.Client client) {
            Supabase = client;
            Supabase.Auth.AddStateChangedListener(AuthEventHandler);

            if (Supabase.Auth.CurrentUser != null)
                IsLoggedIn = true;
        }

        /// <summary>
        /// Registers a listener for change in focused app section.
        /// </summary>
        /// <param name="listener"></param>
        public void AddFocusedSectionChangedListener(IAppStateService.FocusedSectionChanged listener) {
            if (!_focusedSectionChangedListeners.Contains(listener))
                _focusedSectionChangedListeners.Add(listener);
        }

        /// <summary>
        /// Removes a listener for change in focused app section.
        /// </summary>
        /// <param name="listener"></param>
        public void RemoveFocusedSectionChangedListener(IAppStateService.FocusedSectionChanged listener) {
            if (_focusedSectionChangedListeners.Contains(listener))
                _focusedSectionChangedListeners.Remove(listener);
        }

        /// <summary>
        /// Clears all focused app section listeners.
        /// </summary>
        /// <param name="listener"></param>
        public void ClearFocusedSectionChangedListeners(IAppStateService.FocusedSectionChanged listener) =>
            _focusedSectionChangedListeners.Clear();

        /// <summary>
        /// Allows other classes to notify of change in focused app section.
        /// </summary>
        /// <param name="gainedFocus"></param>
        public void SetFocusedSection(IAppStateService.AppSection gainedFocus) {
            var lostFocus = FocusedSection;
            FocusedSection = gainedFocus;

            NotifyFocusedAppSectionChanged(lostFocus, gainedFocus);
        }

        /// <summary>
        /// Notifies listeners of change in app section.
        /// </summary>
        /// <param name="lostFocus"></param>
        /// <param name="gainedFocus"></param>
        private void NotifyFocusedAppSectionChanged(IAppStateService.AppSection lostFocus,
            IAppStateService.AppSection gainedFocus) {
            if (lostFocus == gainedFocus) return;

            foreach (var listener in _focusedSectionChangedListeners.ToList())
                listener.Invoke(lostFocus, gainedFocus);
        }


        ///<inheritdoc />
        private void OnPropertyChanged([CallerMemberName] string? propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        ///<inheritdoc />
        private void SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null) {
            if (EqualityComparer<T>.Default.Equals(field, value)) return;
            field = value;
            OnPropertyChanged(propertyName);
        }

        /// <summary>
        /// Listens with Gotrue for change in authentication state.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="state"></param>
        private void AuthEventHandler(IGotrueClient<User, Session> sender, Constants.AuthState state) {
            IsLoggedIn = state switch {
                Constants.AuthState.SignedIn => true,
                Constants.AuthState.SignedOut => false,
                _ => IsLoggedIn
            };
        }
    }
}
