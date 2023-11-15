using EatMe.Db.Models;
using EatMe.Shared.Utilities;
using System.ComponentModel;

namespace EatMe.Shared.Interfaces {
    public interface IIngredientService : INotifyPropertyChanged {
        bool IsLoading { get; }
        FullyObservableCollection<Ingredient> Ingredients { get; set; }
        Task<bool> Create(Ingredient ingredient);
        Task<bool> Update(Ingredient ingredient);
        Task<bool> Delete(Ingredient ingredient);
    }
}
