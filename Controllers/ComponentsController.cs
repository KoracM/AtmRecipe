using AtmRecipeApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AtmRecipeApp.Controllers
{
    public class ComponentsController : Controller
    {
        // Database bağlantısı (Dependency Injection)
        private readonly AtmRecipeContext _context;

        public ComponentsController(AtmRecipeContext context)
        {
            _context = context;
        }

        // GET: Components - Bileşen listesi
        public async Task<IActionResult> Index()
        {
            var components = await _context.Components.ToListAsync();
            return View(components); // Components listesini view'e gönder
        }

        // GET: Components/Details/5 - Bileşen detayı
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var component = await _context.Components.FindAsync(id);
            if (component == null)
            {
                return NotFound();
            }

            return View(component);
        }

        // GET Create - Boş form göster
        public IActionResult Create()
        {
            return View(); // Create.cshtml sayfası döner
        }

        // POST Create - Formdan gelen veriyi kaydet
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Component component)
        {
            // Model doğrulama (bileşen adı ve fiyat boş mu vb.)
            if (ModelState.IsValid)
            {
                _context.Components.Add(component); // Yeni bileşeni ekle
                await _context.SaveChangesAsync(); // Veritabanına kaydet
                return RedirectToAction("Index"); // Bileşen listesine dön
            }
            return View(component); // Hata varsa formu tekrar göster
        }

        // GET Edit - Güncelleme formu göster (mevcut veriyle dolu)
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var component = await _context.Components.FindAsync(id);
            if (component == null)
            {
                return NotFound();
            }
            return View(component); // Edit.cshtml'e bileşeni gönder
        }

        // POST Edit - Güncellemeyi kaydet
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Component component)
        {
            if (id != component.Id) // URL'deki ID ile form'daki ID eşleşiyor mu?
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(component); // Bileşeni güncelle
                    await _context.SaveChangesAsync(); // Database'e kaydet
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ComponentExists(component.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index"); // Listeye dön
            }
            return View(component); // Hata varsa formu tekrar göster
        }

        // GET Delete - Silme onay sayfası
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var component = await _context.Components
                .FirstOrDefaultAsync(m => m.Id == id);
            if (component == null)
            {
                return NotFound();
            }

            return View(component); // Delete.cshtml'e bileşeni gönder
        }

        // POST Delete - Silme işlemini gerçekleştir
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var component = await _context.Components.FindAsync(id);
            if (component != null)
            {
                _context.Components.Remove(component); // Bileşeni sil
                await _context.SaveChangesAsync(); // Database'e kaydet
            }

            return RedirectToAction("Index"); // Listeye dön
        }

        // Yardımcı metod - Bileşen var mı kontrol et
        private bool ComponentExists(int id)
        {
            return _context.Components.Any(e => e.Id == id);
        }
    }
}
