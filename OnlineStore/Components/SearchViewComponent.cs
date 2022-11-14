using Microsoft.AspNetCore.Mvc;
using OnlineStore.Models;
using Microsoft.EntityFrameworkCore;


namespace OnlineStore.Components
{
    public class SearchViewComponent: ViewComponent
    {
        ApplicationContext db;
        public SearchViewComponent(ApplicationContext context)
        {
            this.db = context;
        }

        public IViewComponentResult Invoke()
        {
            return View();
        }

        
    }
}
