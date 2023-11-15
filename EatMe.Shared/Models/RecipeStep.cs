#nullable enable
using Postgrest.Attributes;
using Postgrest.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EatMe.Db.Models;

[Table("recipe_steps")]
public class RecipeStep : BaseModel, INotifyPropertyChanged {
    [PrimaryKey("id")]
    public int Id { get; set; }

    [Column("recipe_id")]
    public int? RecipeId { get; set; }

    [Column("ordinal")]
    public int? Ordinal { get; set; }

    [Column("instructions")]
    public string? Instructions { get; set; }

    [Reference(typeof(Recipe))]
    public Recipe? Recipe { get; set; }

    public override bool Equals(object? obj) => obj is RecipeStep recipeStep && Id == recipeStep.Id;

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
