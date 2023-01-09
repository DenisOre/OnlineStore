using Microsoft.AspNetCore.Mvc;
using OnlineStore.Models;
using Microsoft.EntityFrameworkCore;


namespace OnlineStore.Components
{
    public class SearchViewComponent: ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
