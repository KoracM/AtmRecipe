using AtmRecipeApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AtmRecipeApp.Controllers
{
    public class ProductsController : Controller
    {
        // Database bağlantısı (Dependency Injection)
        private readonly AtmRecipeContext _context;

        public ProductsController(AtmRecipeContext context)
        {
            _context = context;
        }

        // FindAll
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products.ToListAsync();
            return View(products); // products listesini view'e gönder
        }

        // FindByID - Ürün detayları ve bileşenleri
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Ürünü ve bileşenlerini çek
            var product = await _context.Products
                .Include(p => p.ProductComponents)
                    .ThenInclude(pc => pc.Component)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET Create. Boş form gösterilir.
        public IActionResult Create()
        {
            return View(); //Create.cshtml sayfası döner
        }

        // javadaki @notasyonlar burada [attributes] olarak kullaniliyor.

        [HttpPost] // Bu metod sadece POST request'lerinde çalışır. javadaki @PostMapping gibi
        [ValidateAntiForgeryToken] // Güvenlik (CSRF koruması)

        //POST Create. Formdan gelen veriyi kaydeder. Yukarıdaki attibuteler bu fonksiyonun 
        //sadece POST request'lerinde çalışmasını ve güvenli olmasını sağlar.
        public async Task<IActionResult> Create(Product product)
        {
            // Model doğrulama (ürün adı boş mu vb.)
            if (ModelState.IsValid)
            {
                _context.Products.Add(product); // Yeni ürünü ekle
                await _context.SaveChangesAsync(); // Veritabanına kaydet
                return RedirectToAction("Index"); // Ürün listesine dön
            }
            return View(product); // Hata varsa formu tekrar göster
        }

        // GET Edit - Güncelleme formu göster (mevcut veriyle dolu)
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id); // Ürünü bul
            if (product == null)
            {
                return NotFound();
            }
            return View(product); // Edit.cshtml'e ürünü gönder
        }

        // POST Edit - Güncellemeyi kaydet
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            if (id != product.Id) // URL'deki ID ile form'daki ID eşleşiyor mu?
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product); // Ürünü güncelle
                    await _context.SaveChangesAsync(); // Database'e kaydet
                }
                catch (DbUpdateConcurrencyException) // Aynı anda başkası güncellerse
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw; // Başka bir hata
                    }
                }
                return RedirectToAction("Index"); // Listeye dön
            }
            return View(product); // Hata varsa formu tekrar göster
        }

        // GET Delete - Silme onay sayfası
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product); // Delete.cshtml'e ürünü gönder (onay için)
        }

        // POST Delete - Silme işlemini gerçekleştir
        [HttpPost, ActionName("Delete")] // ActionName = URL'de "Delete" olarak görünsün
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product); // Ürünü sil
                await _context.SaveChangesAsync(); // Database'e kaydet
            }

            return RedirectToAction("Index"); // Listeye dön
        }

        // Yardımcı metod - Ürün var mı kontrol et
        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }

        // GET: Products/AddComponent/5 - Ürüne bileşen ekleme formu
        public async Task<IActionResult> AddComponent(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            // Tüm bileşenleri ViewBag ile gönder
            ViewBag.ProductId = id;
            ViewBag.ProductName = product.Name;
            ViewBag.Components = await _context.Components.ToListAsync();

            return View();
        }

        // POST: Products/AddComponent - Bileşeni ürüne ekle
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComponent(int productId, int componentId, int quantity)
        {
            if (quantity <= 0)
            {
                ModelState.AddModelError("", "Miktar 0'dan büyük olmalıdır.");
                ViewBag.ProductId = productId;
                ViewBag.Components = await _context.Components.ToListAsync();
                return View();
            }

            // Aynı bileşen zaten ekliyse güncelle
            var existing = await _context.ProductComponents
               .FirstOrDefaultAsync(pc => pc.ProductId == productId && pc.ComponentId == componentId);

            if (existing != null)
            {
                existing.Quantity = quantity;
                _context.Update(existing);
            }
            else
            {
                var productComponent = new ProductComponent
                {
                    ProductId = productId,
                    ComponentId = componentId,
                    Quantity = quantity,
                    Product = await _context.Products.FindAsync(productId) ?? throw new InvalidOperationException(),
                    Component = await _context.Components.FindAsync(componentId) ?? throw new InvalidOperationException()
                };

                _context.ProductComponents.Add(productComponent);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Details", new { id = productId });
        }

        // POST: Products/RemoveComponent - Bileşeni üründen çıkar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveComponent(int productId, int componentId)
        {
            var productComponent = await _context.ProductComponents
                .FirstOrDefaultAsync(pc => pc.ProductId == productId && pc.ComponentId == componentId);

            if (productComponent != null)
            {
                _context.ProductComponents.Remove(productComponent);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Details", new { id = productId });
        }
    }
}