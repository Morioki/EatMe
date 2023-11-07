#nullable enable
using Postgrest.Attributes;
using Postgrest.Models;

namespace EatMe.Db.Models;

[Table("recipes")]
public class Recipe : BaseModel {
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
}
