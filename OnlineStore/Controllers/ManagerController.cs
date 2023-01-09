using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Models;





namespace OnlineStore.Controllers
{
    
    public class ManagerController : Controller
    {
        ApplicationContext db;
        public ManagerController(ApplicationContext context)
        {
            db = context;
        }

        
        public async Task<IActionResult> Manager()
        {
            AccountViewModel accountViewModel = new AccountViewModel();
            accountViewModel.readyOrders = await db.Orders.Include(o => o.User).Include(o => o.products).Where(o => (o.isRegistered == true) && (o.isReady == true) && (o.isPaid == false)).ToListAsync();
            if (accountViewModel.readyOrders != null)
            {
                foreach (var item in accountViewModel.readyOrders)
                {
                    item.products = getBuysOfOrder(item.Id);
                }
            }
            else
            {
                ViewBag.ManagerReadyOrdersInfo = "Нет готовых к выдаче заказов";
            }
            //---------------------------
            accountViewModel.inWorkOrders = await db.Orders.Include(o => o.User).Include(o => o.products).Where(o => (o.isRegistered == true) && (o.isReady == false)).ToListAsync();
            if (accountViewModel.inWorkOrders != null)
            {
                foreach (var item in accountViewModel.inWorkOrders)
                {
                    item.products = getBuysOfOrder(item.Id);
                }
            }
            else
            {
                ViewBag.ManagerInWorkOrdersInfo = "Нет заказов на комплектации";
            }
            //----------------------------
            accountViewModel.paidOrders = await db.Orders.Include(o => o.User).Include(o => o.products).Where(o => (o.isRegistered == true) && (o.isReady == true) && (o.isPaid == true)).ToListAsync();
            if (accountViewModel.paidOrders != null)
            {
                foreach (var item in accountViewModel.paidOrders)
                {
                    item.products = getBuysOfOrder(item.Id);
                }
            }
            else
            {
                ViewBag.ManagerPaidOrdersInfo = "Нет завершенных заказов";
            }
            return View("ManagerOrders", accountViewModel);
        }

        
        [HttpPost]
        public async Task<IActionResult> ChangeOrderStatusToReady(int? Id)
        {
            Order? order = await db.Orders.FirstOrDefaultAsync(o => o.Id == Id);
            if (order != null)
            {
                order.isReady = true;
                db.Orders.Update(order);
                await db.SaveChangesAsync();
            }
            return RedirectToAction("Manager");
        }

        
        [HttpPost]
        public async Task<IActionResult> ChangeOrderStatusToPaid(int? Id)
        {
            Order? order = await db.Orders.FirstOrDefaultAsync(o => o.Id == Id);
            if (order != null)
            {
                order.isPaid = true;
                db.Orders.Update(order);
                await db.SaveChangesAsync();
            }
            return RedirectToAction("Manager");
        }






        private List<Buy> getBuysOfOrder(int Id)
        {
            List<Buy> buys = db.Buys.Include(b => b.Product).Where(b => b.OrderId == Id).ToList();
            return buys;
        }
    }
}
