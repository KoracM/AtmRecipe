using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AtmRecipeApp.Models;

namespace AtmRecipeApp.Controllers;

public class RecipeController : Controller //springboottaki @Controller gibi   
{
    //Not: Readonly ile deger sadece constructor icerisinde atanir. Sonradan degismez.
    private readonly AtmRecipeContext _context; //_context db baglantisi tutar

    // Constructor - Database bağlantısını aliyor
    public RecipeController(AtmRecipeContext context)
    {
        _context = context;
    }

    // Ana sayfa - Ürünleri listele
    // Asenkron metod nedir?
    public async Task<IActionResult> Index()
    {
        // Tüm ürünleri database'den çek. repository.findAll() gibi
        // await ile db'den yanit gelene kadar bekliyoruz. 
        var products = await _context.Products.ToListAsync(); //ToList kullanilsaydi sorgu bitene kadar thread baska bir is yapamazdi. 
        return View(products); //view ile html sayfasi dondurur. 
    }

    // Seçilen ürünün reçetesini göster
    public async Task<IActionResult> ShowRecipe(int productId)
    {
        // Ürünü bul (ID ile arama - Spring'deki findById() gibi)
        var product = await _context.Products.FindAsync(productId);

        // Eğer ürün bulunamazsa NotFound() döndürür
        if (product == null)
        {
            return NotFound();
        }

        // Bu ürünün bileşenlerini ve miktarlarını çek
        var recipe = await _context.ProductComponents
            .Where(pc => pc.ProductId == productId)
            .Include(pc => pc.Component) // Component bilgilerini de getir
            .ToListAsync();

        // Toplam fiyatı hesapla
        decimal totalPrice = 0;
        foreach (var item in recipe)
        {
            totalPrice += item.Component.Price * item.Quantity;
        }

        // View'a verileri gönder
        ViewBag.ProductName = product.Name;
        ViewBag.TotalPrice = totalPrice;

        return View(recipe);
    }
}
