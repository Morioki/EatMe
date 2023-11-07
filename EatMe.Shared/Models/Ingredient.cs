#nullable enable
using Postgrest.Attributes;
using Postgrest.Models;

namespace EatMe.Db.Models;

[Table("ingredients")]
public class Ingredient : BaseModel {
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
}
