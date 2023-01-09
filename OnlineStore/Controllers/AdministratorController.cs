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
        public IActionResult AdministratorMain()
        {
            return View();
        }
    }
}
