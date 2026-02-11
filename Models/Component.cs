using System.ComponentModel.DataAnnotations;

namespace AtmRecipeApp.Models;

public class Component
{
    public int Id { get; set; }

    [Required]
    public required string Name { get; set; }

    [Required]
    public required decimal Price { get; set; }

    // Navigation property for the many-to-many relationship with Product
    // = new List<ProductComponent>() to ensure it's initialized and avoid null reference issues
    public ICollection<ProductComponent> ProductComponents { get; set; } = new List<ProductComponent>();
}