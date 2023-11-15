using EatMe.Db.Models;
using EatMe.Shared.Interfaces;
using EatMe.Shared.Utilities;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EatMe.Shared.Services {
    public sealed class RecipeService : IRecipeService {

        public FullyObservableCollection<Recipe> Recipes { get; set; } = new();

        public bool _isLoading;

        public bool IsLoading {
            get => _isLoading;
            private set => SetField(ref _isLoading, value);
        }

        /// <inheritdoc/>
        public event PropertyChangedEventHandler? PropertyChanged;






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
