using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Models;
using Microsoft.AspNetCore.Authorization;

namespace OnlineStore.Controllers
{
    [Authorize(Policy = "OnlyForAdmin")]
    public class AdministratorCategoriesController: Controller
    {
        ApplicationContext db;
        public AdministratorCategoriesController(ApplicationContext context) 
        {
            db = context;
        }

        public async Task<IActionResult> AdministratorCategories()
        {            
            List<Category> categories = await db.Categories.ToListAsync();
            categories.Sort();
            List<CategoryIsSelect> categoriesIsSelect = new List<CategoryIsSelect>();
            for (int i = 0; i < categories.Count; i++)
            {
                categoriesIsSelect.Add(new CategoryIsSelect(categories[i]));
            }
            CategoryViewModel categoryViewModel = new CategoryViewModel { categoryIsSelects = categoriesIsSelect };
            return View(categoryViewModel);
        }

        public IActionResult AdministratorAddCategory()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AdministratorAddCategory(string Name)
        {
            Category? cheackCategory = await db.Categories.FirstOrDefaultAsync(c => c.Name == Name);
            if (cheackCategory != null)
            {
                ViewBag.ErrorName = "Ошибка: Такая группа уже существует";
                return View();
            }
            Category category = new Category();
            category.Name = Name;
            db.Categories.Add(category);
            await db.SaveChangesAsync();
            return RedirectToAction("AdministratorCategories");
        }

        [HttpPost]
        public async Task<IActionResult> AdministratorDeleteCategories(CategoryViewModel selectedCategoies)
        {
            Category category;
            foreach (var item in selectedCategoies.categoryIsSelects)
            {
                if (item.IsSelected == true)
                {
                    category = await db.Categories.FirstOrDefaultAsync(c => c.Name == item.Name);
                    if (category != null)
                    {
                        db.Categories.Remove(category);
                        await db.SaveChangesAsync();
                    }
                }                
            }
            return RedirectToAction("AdministratorCategories");            
        }
        
        public async Task<IActionResult> AdministratorOpenEditCategory(string? Name)
        {
            Category? category = await db.Categories.FirstOrDefaultAsync(c => c.Name == Name);
            @ViewBag.ErrorName = "";
            return View("AdministratorEditCategory",category);
        }

        [HttpPost]
        public async  Task<IActionResult> AdministratorEditCategory(Category category)
        {
            db.Categories.Update(category);
            await db.SaveChangesAsync();
            return RedirectToAction("AdministratorCategories");            
        }
    }
}
