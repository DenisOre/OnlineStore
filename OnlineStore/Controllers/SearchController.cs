using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Models;


namespace OnlineStore.Controllers
{
    public class SearchController: Controller
    {
        ApplicationContext db;
        public SearchController(ApplicationContext context)
        {
            this.db = context;
        }

        [HttpPost]
        public async Task<IActionResult> SearchResult(string searchText)
        {
            string search = searchText.Trim();
            List<Product> products = await db.Products.Include(p => p.imagesProduct).Where(p => p.Name == search).ToListAsync();
            if (products.Count != 0)
            {
                return View("~/Views/Home/Index.cshtml", products);
            }

            string[] searchArray = search.Split(new char[] { ' ' },StringSplitOptions.RemoveEmptyEntries);

            List<Product> productsByTwoWordsCategoryAndManufacturer = new List<Product>();
            if (searchArray.Length > 1)
            {
                productsByTwoWordsCategoryAndManufacturer = await db.Products.Include(p => p.imagesProduct).Where(p => p.Category.Name.Substring(0, p.Category.Name.Length - 1) == searchArray[0] && p.Manufacturer.Name == searchArray[1]).ToListAsync();
            }
            if (productsByTwoWordsCategoryAndManufacturer.Count != 0)
            {
                return View("~/Views/Home/Index.cshtml", productsByTwoWordsCategoryAndManufacturer);
            }
            List<Product> productsByFirstWordCategory = await db.Products.Include(p=>p.imagesProduct).Where(p=>p.Category.Name.Substring(0, p.Category.Name.Length-1) == searchArray[0]).ToListAsync();
            if (productsByFirstWordCategory.Count != 0)
            {
                return View("~/Views/Home/Index.cshtml", productsByFirstWordCategory);
            }
            List<Product> productsByFirstWordManufacturer = await db.Products.Include(p => p.imagesProduct).Where(p => p.Manufacturer.Name == searchArray[0]).ToListAsync();
            if (productsByFirstWordManufacturer.Count != 0)
            {
                return View("~/Views/Home/Index.cshtml", productsByFirstWordManufacturer);
            }

            string message = "По вашему запросу ничего не найдено.";
            return View("~/Views/Home/SearchNullResult.cshtml", message);
        }
    }
}
