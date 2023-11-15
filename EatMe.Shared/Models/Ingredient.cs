#nullable enable
using Postgrest.Attributes;
using Postgrest.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EatMe.Db.Models;

[Table("ingredients")]
public class Ingredient : BaseModel, INotifyPropertyChanged {
    [PrimaryKey("id")]
    public int Id { get; set; }

    [Column("name")]
    public string? Name { get; set; }

    [Column("size")]
    public string? Size { get; set; }

    [Column("opened")]
    public bool? Opened { get; set; }

    [Column("date_expiration")]
    public DateTime? DateExpiration { get; set; }

    [Column("date_purchased")]
    public DateTime? DatePurchased { get; set; }

    [Reference(typeof(RecipeIngredient))]
    public List<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();

    public override bool Equals(object? obj) =>
        obj is Ingredient ingredient && Id == ingredient.Id;

    public override int GetHashCode() => HashCode.Combine(Id);

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
