using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Models;


namespace OnlineStore.Controllers
{
    public class CatalogController: Controller
    {
        ApplicationContext db;
        public CatalogController(ApplicationContext context)
        {
            this.db = context;
        }

        public async Task<IActionResult> CatalogSelectCategory(int Id)
        {
            List<Product> products = await db.Products.Include(p => p.imagesProduct).Where(p => p.Category.Id == Id).ToListAsync();
            return View("~/Views/Home/Index.cshtml", products);
        }

    }
}
