using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Models;
using Microsoft.AspNetCore.Authorization;


namespace OnlineStore.Controllers
{
    [Authorize]
    public class AccountController: Controller
    {
        ApplicationContext db;
        public AccountController(ApplicationContext context)
        {
            db = context;
        }

        public async Task<IActionResult> Account()
        {
            User? user = await db.Users.FirstOrDefaultAsync(u => u.Email == User.Identity.Name);
            if (user != null)
            {
                AccountViewModel accountViewModel = new AccountViewModel();
                accountViewModel.readyOrders = await db.Orders.Include(o => o.User).Include(o => o.products).Where(o => (o.User.Id == user.Id) && (o.isRegistered == true) && (o.isReady == true) && (o.isPaid == false)).ToListAsync();
                if (accountViewModel.readyOrders != null)
                {
                    foreach (var item in accountViewModel.readyOrders)
                    {
                        item.products = getBuysOfOrder(item.Id);
                    }
                }
                
                accountViewModel.inWorkOrders = await db.Orders.Include(o => o.User).Include(o => o.products).Where(o => (o.User.Id == user.Id) && (o.isRegistered == true) && (o.isReady == false)).ToListAsync();
                if (accountViewModel.inWorkOrders != null)
                {
                    foreach (var item in accountViewModel.inWorkOrders)
                    {
                        item.products = getBuysOfOrder(item.Id);
                    }
                }
                
                accountViewModel.paidOrders = await db.Orders.Include(o => o.User).Include(o => o.products).Where(o => (o.User.Id == user.Id) && (o.isRegistered == true) && (o.isReady == true) && (o.isPaid == true)).ToListAsync();
                if (accountViewModel.paidOrders != null)
                {
                    foreach (var item in accountViewModel.paidOrders)
                    {
                        item.products = getBuysOfOrder(item.Id);
                    }
                }               
                return View(accountViewModel);
            }
            return RedirectToAction("Index", "Home");
        }








        private List<Buy> getBuysOfOrder(int Id)
        {
            List<Buy> buys = db.Buys.Include(b=>b.Product).Where(b => b.OrderId == Id).ToList();
            return buys;
        }

    }
}
