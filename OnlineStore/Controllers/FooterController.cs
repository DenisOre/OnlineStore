using Microsoft.AspNetCore.Mvc;

namespace OnlineStore.Controllers
{
    public class FooterController: Controller
    {
        public IActionResult Contacts()
        { 
            return View();
        }

        public IActionResult Payment()
        {
            return View();
        }

        public IActionResult PointOfIssue()
        {
            return View();
        }
    }
}
