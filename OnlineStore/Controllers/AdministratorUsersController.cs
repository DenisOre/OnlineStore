using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Models;
using Microsoft.AspNetCore.Authorization;

namespace OnlineStore.Controllers
{
    [Authorize(Policy = "OnlyForAdmin")]
    public class AdministratorUsersController: Controller
    {
        ApplicationContext db;
        public AdministratorUsersController(ApplicationContext context)
        {
            db = context;
        }

        public async Task<IActionResult> AdministratorUsers()
        {
            List<User> users = await db.Users.Include(r => r.Role).ToListAsync();
            users.Sort();
            List<UserIsSelect> usersIsSelect = new List<UserIsSelect>();
            for (int i = 0; i < users.Count; i++)
            {
                usersIsSelect.Add(new UserIsSelect(users[i]));
            }
            UserViewModel userViewModel = new UserViewModel { userIsSelect = usersIsSelect };
            return View(userViewModel);
        }

        public async Task<IActionResult> AdministratorAddUser()
        {
            List<Role> roles = await db.Roles.ToListAsync();
            roles.Sort();
            return View(roles);
        }

        [HttpPost]
        public async Task<IActionResult> AdministratorAddUser(User user, string Role)
        {
            User? checkUser = await db.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
            if(checkUser != null)
            {
                ViewBag.ErrorEmail = "Ошибка: Такой пользователь уже существует";
                return View();
            }
            User newUser = user;
            Role? role = await db.Roles.FirstOrDefaultAsync(r => r.Name == Role);
            newUser.Role = role;
            db.Users.Add(newUser);
            await db.SaveChangesAsync();
            return RedirectToAction("AdministratorUsers");
        }

        [HttpPost]
        public async Task<IActionResult> AdministratorDeleteUsers(UserViewModel selectedUsers)
        {
            User? user;
            foreach (var item in selectedUsers.userIsSelect)
            {
                if (item.IsSelected == true)
                {
                    user = await db.Users.FirstOrDefaultAsync(u => u.Email == item.Email);
                    if (user != null)
                    {
                        db.Users.Remove(user);
                        await db.SaveChangesAsync();
                    }
                }
            }
            return RedirectToAction("AdministratorUsers");
        }

        public async Task<IActionResult> AdministratorOpenEditUser(int? Id)
        {
            User? user = await db.Users.FirstOrDefaultAsync(u => u.Id == Id);
            List<Role> roles = await db.Roles.ToListAsync();
            UserEditViewModel userEditViewModel = new UserEditViewModel();
            if (user != null)
            {
                userEditViewModel.User = user;
                userEditViewModel.Roles = roles;
            }
            return View("AdministratorEditUser", userEditViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AdministratorEditUser(User user, string Role)
        {
            Role? role = await db.Roles.FirstOrDefaultAsync(r => r.Name == Role);
            user.Role = role;
            db.Users.Update(user);
            await db.SaveChangesAsync();
            return RedirectToAction("AdministratorUsers");
        }

    }
}
