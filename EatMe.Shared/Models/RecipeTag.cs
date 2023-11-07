#nullable enable
using Postgrest.Attributes;
using Postgrest.Models;

namespace EatMe.Db.Models;

[Table("recipe_tags")]
public class RecipeTag : BaseModel {
    [PrimaryKey("id")]
    public int Id { get; set; }

    [Column("recipe_id")]
    public int? RecipeId { get; set; }

    [Column("ordinal")]
    public int? Ordinal { get; set; }

    [Column("tag")]
    public string? Tag { get; set; }

    [Reference(typeof(Recipe))]
    public Recipe? Recipe { get; set; }

    public override bool Equals(object? obj) => obj is RecipeTag recipeTag && Id == recipeTag.Id;

    public override int GetHashCode() => HashCode.Combine(Id);
}
