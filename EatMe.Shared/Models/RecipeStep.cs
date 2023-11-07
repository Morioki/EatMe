#nullable enable
using Postgrest.Attributes;
using Postgrest.Models;

namespace EatMe.Db.Models;

[Table("recipe_steps")]
public class RecipeStep : BaseModel {
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
}
