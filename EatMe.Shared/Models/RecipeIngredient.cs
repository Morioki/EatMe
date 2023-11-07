#nullable enable
using Postgrest.Attributes;
using Postgrest.Models;

namespace EatMe.Db.Models;

public class RecipeIngredient : BaseModel {
    [PrimaryKey("id")]
    public int Id { get; set; }

    [Column("ingredient_id")]
    public int? IngredientId { get; set; }

    [Column("recipe_id")]
    public int? RecipeId { get; set; }

    [Column("amount")]
    public double? Amount { get; set; }

    [Column("measurement")]
    public string? Measurement { get; set; }

    [Column("notes")]
    public string? Notes { get; set; }

    [Reference(typeof(Ingredient))]
    public Ingredient? Ingredient { get; set; }

    [Reference(typeof(Recipe))]
    public virtual Recipe? Recipe { get; set; }

    public override bool Equals(object? obj) => obj is RecipeIngredient rIngred && Id == rIngred.Id;

    public override int GetHashCode() => HashCode.Combine(Id);
}
