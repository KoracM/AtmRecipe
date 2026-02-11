using Microsoft.EntityFrameworkCore;

namespace AtmRecipeApp.Models;

public class AtmRecipeContext : DbContext
{
    // Constructor (DbContextOptions is used to configure the database connection)
    public AtmRecipeContext(DbContextOptions<AtmRecipeContext> options) : base(options)
    {
    }

    // DbSet definitions (tables)
    public required DbSet<Product> Products { get; set; }
    public required DbSet<Component> Components { get; set; }
    public required DbSet<ProductComponent> ProductComponents { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Product tablosu 
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Name).IsRequired();
        });

        // Component tablosu
        modelBuilder.Entity<Component>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Name).IsRequired();
            entity.Property(c => c.Price).IsRequired();
        });

        // ProductComponent tablosu
        modelBuilder.Entity<ProductComponent>(entity =>
        {
            entity.HasKey(pc => pc.Id);

            entity.Property(pc => pc.ProductId).IsRequired();
            entity.Property(pc => pc.ComponentId).IsRequired();
            entity.Property(pc => pc.Quantity).IsRequired();

            // ProductId foreign key tanımı (ProductComponents -> Products)
            entity.HasOne(pc => pc.Product)                    // ProductComponent'in bir Product'ı var
                .WithMany(p => p.ProductComponents)            // Product'ın birden çok ProductComponent'i var
                .HasForeignKey(pc => pc.ProductId)             // Foreign Key: ProductId
                .OnDelete(DeleteBehavior.Cascade);             // Product silinirse, ProductComponent da silinir

            // ComponentId foreign key tanımı (ProductComponents -> Components)
            entity.HasOne(pc => pc.Component)                  // ProductComponent'in bir Component'i var
                .WithMany(c => c.ProductComponents)            // Component'in birden çok ProductComponent'i var
                .HasForeignKey(pc => pc.ComponentId)           // Foreign Key: ComponentId
                .OnDelete(DeleteBehavior.Cascade);             // Component silinirse, ProductComponent da silinir
        });
    }
}