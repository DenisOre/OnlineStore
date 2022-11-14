using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Models;


namespace OnlineStore.Controllers
{
    public class CardOfProductController:Controller
    {
        ApplicationContext db;
        public CardOfProductController(ApplicationContext context)
        {
            db = context;
        }
        public async Task<IActionResult> CardOfProduct(int? Id)
        {
            Product? product = await db.Products.Include(p=>p.imagesProduct).FirstOrDefaultAsync(p => p.Id == Id);
            return View(product);
        }
    }
}
