#nullable enable
using Postgrest.Attributes;
using Postgrest.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EatMe.Db.Models;

[Table("recipes")]
public class Recipe : BaseModel, INotifyPropertyChanged {
    [PrimaryKey("id")]
    public int Id { get; set; }

    [Column("description")]
    public string? Description { get; set; }

    [Column("servings")]
    public double? Servings { get; set; }

    /// <summary>
    /// In Minutes
    /// </summary>
    [Column("prep_time")]
    public double? PrepTime { get; set; }

    /// <summary>
    /// In Minutes
    /// </summary>
    [Column("cook_time")]
    public double? CookTime { get; set; }

    [Reference(typeof(RecipeIngredient))]
    public List<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();

    [Reference(typeof(RecipeStep))]
    public List<RecipeStep> RecipeSteps { get; set; } = new List<RecipeStep>();

    [Reference(typeof(RecipeTag))]
    public List<RecipeTag> RecipeTags { get; set; } = new List<RecipeTag>();

    public override bool Equals(object? obj) => obj is Recipe recipe && Id == recipe.Id;

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
