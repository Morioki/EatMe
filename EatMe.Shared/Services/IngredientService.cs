using EatMe.Db.Models;
using EatMe.Shared.Interfaces;
using EatMe.Shared.Utilities;
using Postgrest.Exceptions;
using Postgrest.Interfaces;
using Supabase.Realtime;
using Supabase.Realtime.Exceptions;
using Supabase.Realtime.Interfaces;
using Supabase.Realtime.PostgresChanges;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EatMe.Shared.Services {

    /// <summary>
    /// The Ingredient service handles maintaining local copy of the Ingredients in the system.
    /// </summary>
    public sealed class IngredientService : IIngredientService {

        private bool _isLoading;

        /// <summary>
        /// If the service is currently loading.
        /// </summary>
        public bool IsLoading {
            get => _isLoading;
            private set => SetField(ref _isLoading, value);
        }

        /// <summary>
        /// Stored collection of Ingredients.
        /// </summary>
        public FullyObservableCollection<Ingredient> Ingredients { get; set; } = new();

        /// <inheritdoc/>
        public event PropertyChangedEventHandler? PropertyChanged;

        private Supabase.Client Supabase { get; }
        private IAppStateService AppStateService { get; }
        private IPostgrestCacheProvider PostgrestCacheProvider { get; }
        private IPostgrestTableWithCache<Ingredient> Ref => Supabase.Postgrest.Table<Ingredient>(PostgrestCacheProvider);
        private RealtimeChannel? Listener { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="appStateService"></param>
        /// <param name="supabase"></param>
        /// <param name="postgrestCacheProvider"></param>
        public IngredientService(IAppStateService appStateService, Supabase.Client supabase,
            IPostgrestCacheProvider postgrestCacheProvider) {
            AppStateService = appStateService;
            Supabase = supabase;
            PostgrestCacheProvider = postgrestCacheProvider;

            AppStateService.PropertyChanged += AppStateServiceOnPropertyChanged;

            if (AppStateService.IsLoggedIn)
                Register();
        }

        // TODO
        public Task<bool> Create(Ingredient ingredient) {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(Ingredient ingredient) {
            throw new NotImplementedException();
        }

        public Task<bool> Update(Ingredient ingredient) {
            throw new NotImplementedException();
        }





        /// <summary>
        /// Handles changes to App State, specifically checking when a user signs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AppStateServiceOnPropertyChanged(object? sender, PropertyChangedEventArgs e) {
            if (AppStateService.IsLoggedIn)
                Register();
            else
                Unregister();
        }

        /// <summary>
        /// Registers the realtime listener and <see cref="Populate"/>s the local cache.
        /// </summary>
        private async void Register() {
            IsLoading = true;

            await Supabase.InitializeAsync();

            try {
                Listener ??= await Supabase.From<Ingredient>().On(PostgresChangesOptions.ListenType.All, OnTodoModelChanges);
            } catch (RealtimeException ex) {
                Console.WriteLine(ex.Message);
                AppStateService.Errors.Add(ex.Message);
            }

            await Populate();

            IsLoading = false;
        }

        /// <summary>
        /// Handles changes registered from the <see cref="Listener"/>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="change"></param>
        private void OnTodoModelChanges(IRealtimeChannel sender, PostgresChangesResponse change) {
            var model = change.Model<Ingredient>();

            switch (change.Payload?.Data?._type) {
                case "INSERT":
                    Console.WriteLine($"Ingredient has been inserted: {model.Id}");
                    var existing = Ingredients.FirstOrDefault(t => t.Id == model.Id);
                    if (existing == null)
                        Ingredients.Add(model);
                    break;
                case "UPDATE":
                    Console.WriteLine($"Ingredient has been updated: {model.Id}");
                    var toBeUpdated = Ingredients.FirstOrDefault(t => t.Id == model!.Id);
                    Ingredients[Ingredients.IndexOf(toBeUpdated)] = model;
                    break;
                case "DELETE":
                    Console.WriteLine($"Ingredient has been deleted: {model.Id}");
                    var toBeRemoved = Ingredients.FirstOrDefault(t => t.Id == model!.Id);
                    Ingredients.Remove(toBeRemoved);
                    break;
            }
        }

        /// <summary>
        /// Removes the realtime listener and clears <see cref="Todos"/>
        /// </summary>
        private void Unregister() {
            Listener?.Unsubscribe();
            Listener = null;
            Ingredients.Clear();
        }

        /// <summary>
        /// Populates local <see cref="Todos"/> with a hard pull from <see cref="Supabase"/>. Initially populates from
        /// the <see cref="PostgrestCacheProvider"/> if possible.
        /// </summary>
        private async Task Populate() {
            try {
                var response = await Ref.Get();

                foreach (var model in response.Models)
                    Ingredients.Add(model);

                response.RemoteModelsPopulated += sender => {
                    foreach (var model in sender.Models) {
                        var existing = Ingredients.FirstOrDefault(t => t.Id == model.Id);
                        Ingredients[Ingredients.IndexOf(existing)] = model;
                    }
                };
            } catch (PostgrestException ex) {
                Console.WriteLine(ex.Message);
                AppStateService.Errors.Add(ex.Message);
            }
        }

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null) {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
