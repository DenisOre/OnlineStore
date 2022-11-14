using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Models;


namespace OnlineStore.Components
{
    public class CatalogViewComponent: ViewComponent
    {
        ApplicationContext db;
        public CatalogViewComponent(ApplicationContext context)
        {
            this.db = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Category> categories = await db.Categories.ToListAsync();
            categories.Sort();
            return View(categories);
        }
    }
}
