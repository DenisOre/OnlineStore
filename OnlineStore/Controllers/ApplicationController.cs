using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Models;


namespace OnlineStore.Controllers
{
    public class ApplicationController: Controller
    {
        ApplicationContext db;
        public ApplicationController(ApplicationContext context)
        {
            db = context;
            ViewBag.categories = db.Categories.ToList();
        }
    }
}
