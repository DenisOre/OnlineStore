using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using OnlineStore.Models;


namespace OnlineStore.Controllers
{
    public class EnterOrRegistrationController: Controller
    {
        ApplicationContext db;
        public EnterOrRegistrationController(ApplicationContext context)
        {
            this.db = context;
        }

        public IActionResult Enter()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Enter(string EmailEnter, string PasswordEnter)
        {
            User? user = await db.Users.Include(u=>u.Role).FirstOrDefaultAsync(u => u.Email == EmailEnter && u.Password == PasswordEnter);
            if (user != null)
            {
                await Authenticate(user);
                return RedirectToAction("Index", "Home");
            }
            ViewBag.ErrorMessage = "Неверный логин или пароль!!!";
            return View();
        }

        public async Task<IActionResult> Exit()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registration(User user, string PasswordRepeat)
        {
            User? checkUser = await db.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
            if (checkUser == null)
            {
                user.Role = await db.Roles.FirstOrDefaultAsync(r => r.Name == "Пользователь");
                db.Users.Add(user);
                await db.SaveChangesAsync();
                @ViewBag.ErrorMessage = "Вы успешно прошли регистрацию!!! Войдите, используя указанный вами Email и пароль.";
                return RedirectToAction("Enter", "EnterOrRegistration");
            }
            ViewBag.ErrorMessage = "Ошибка: пользователь с таким Email уже зарегистрирован!!!";
            return View();
        }



        private async Task Authenticate(User user)
        {
            if (user.Email != null && user.Role != null && user.Role.Name != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.Name)
                };
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookies", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
            }            
        }
    }
}
