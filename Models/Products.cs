using System.ComponentModel.DataAnnotations;

namespace AtmRecipeApp.Models;

public class Product
{
    public int Id { get; set; }

    [Required]
    public required string Name { get; set; }

    // one-to-many relationship with Component through ProductComponent
    // = new List<ProductComponent>() to ensure it's initialized and avoid null reference issues
    public ICollection<ProductComponent> ProductComponents { get; set; } = new List<ProductComponent>();
}