using EatMe.Db.Models;
using EatMe.Shared.Utilities;
using System.ComponentModel;

namespace EatMe.Shared.Interfaces {
    public interface IRecipeService : INotifyPropertyChanged {
        bool IsLoading { get; }
        FullyObservableCollection<Recipe> Recipes { get; set; }
    }
}
