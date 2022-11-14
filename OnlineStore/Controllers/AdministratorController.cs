using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;


namespace OnlineStore.Controllers
{
    [Authorize(Policy = "OnlyForAdmin")]
    public class AdministratorController: Controller
    {
        ApplicationContext db;
        public AdministratorController(ApplicationContext context)
        {
            this.db = context;
            ViewBag.categories = db.Categories.ToList();
        }
        
        public IActionResult AdministratorMain()
        {
            return View();
        }
    }
}
