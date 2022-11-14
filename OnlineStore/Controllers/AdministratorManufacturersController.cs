using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Models;
using Microsoft.AspNetCore.Authorization;

namespace OnlineStore.Controllers
{
    [Authorize(Policy = "OnlyForAdmin")]
    public class AdministratorManufacturersController: Controller
    {
        ApplicationContext db;
        public AdministratorManufacturersController(ApplicationContext context)
        {
            db = context;
        }


        public async Task<IActionResult> AdministratorManufacturers()
        {
            List<Manufacturer> manufacturers = await db.Manufacturers.ToListAsync();
            manufacturers.Sort();
            List<ManufacturerIsSelect> manufacturersIsSelect = new List<ManufacturerIsSelect>();
            for (int i = 0; i < manufacturers.Count; i++)
            {
                manufacturersIsSelect.Add(new ManufacturerIsSelect(manufacturers[i]));
            }
            ManufacturerViewModel manufacturerViewModel = new ManufacturerViewModel{ manufacturerIsSelects = manufacturersIsSelect };
            return View(manufacturerViewModel);
        }

        public IActionResult AdministratorAddManufacturer()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AdministratorAddManufacturer(string Name)
        {
            Manufacturer cheackManufacturer = await db.Manufacturers.FirstOrDefaultAsync(m => m.Name == Name);
            if (cheackManufacturer != null)
            {
                ViewBag.ErrorName = "Ошибка: Такой производитель уже существует";
                return View();
            }
            Manufacturer manufacturer = new Manufacturer();
            manufacturer.Name = Name;
            db.Manufacturers.Add(manufacturer);
            await db.SaveChangesAsync();
            return RedirectToAction("AdministratorManufacturers");
        }

        [HttpPost]
        public async Task<IActionResult> AdministratorDeleteManufacturer(ManufacturerViewModel selectedManufacturers)
        {
            Manufacturer? manufacturer;
            foreach (var item in selectedManufacturers.manufacturerIsSelects)
            {
                if (item.IsSelected == true)
                {
                    manufacturer = await db.Manufacturers.FirstOrDefaultAsync(m => m.Name == item.Name);
                    if (manufacturer != null)
                    {
                        db.Manufacturers.Remove(manufacturer);
                        await db.SaveChangesAsync();
                    }
                }
            }
            return RedirectToAction("AdministratorManufacturers");
        }

        public async Task<IActionResult> AdministratorOpenEditManufacturer(string Name)
        {
            Manufacturer? manufacturer = await db.Manufacturers.FirstOrDefaultAsync(m => m.Name == Name);
            ViewBag.ErrorName = "";
            return View("AdministratorEditManufacturer",manufacturer);
        }

        [HttpPost]
        public async Task<IActionResult> AdministratorEditManufacturer(Manufacturer manufacturer)
        {
            db.Manufacturers.Update(manufacturer);
            await db.SaveChangesAsync();
            return RedirectToAction("AdministratorManufacturers");
        }
    }
}
