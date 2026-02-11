using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AtmRecipeApp.Models;

public class ProductComponent
{
    public int Id { get; set; }

    [Required]
    public int ProductId { get; set; }

    [Required]
    public required Product Product { get; set; }

    [Required]
    public int ComponentId { get; set; }

    [Required]
    public required Component Component { get; set; }

    [Required]
    public int Quantity { get; set; }
}