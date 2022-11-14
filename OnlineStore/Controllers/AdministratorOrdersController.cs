using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Models;
using Microsoft.AspNetCore.Authorization;



namespace OnlineStore.Controllers
{
    [Authorize(Policy = "OnlyForAdmin")]
    public class AdministratorOrdersController: Controller
    {
        ApplicationContext db;
        public AdministratorOrdersController(ApplicationContext context)
        {
            db = context;
        }

        public async Task<IActionResult> AdministratorOrders()
        {
            List<Order> orders = await db.Orders.Include(o=>o.User).Include(p=>p.products).ToListAsync();
            orders.Sort();
            List<Buy> buys = await db.Buys.Include(p => p.Product).ToListAsync();
            List<OrderIsSelect> ordersIsSelect = new List<OrderIsSelect>();
            for (int i = 0; i < orders.Count; i++)
            {
                ordersIsSelect.Add(new OrderIsSelect(orders[i]));
            }
            OrderViewModel orderViewModel = new OrderViewModel();
            orderViewModel.orderIsSelects = ordersIsSelect;
            orderViewModel.buys = buys;
            return View(orderViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AdministratorDeleteOrder(OrderViewModel orderViewModel)
        {
            Order? order;
            List<Buy> buys = new List<Buy>();
            foreach (var item in orderViewModel.orderIsSelects)
            {
                if (item.IsSelect == true)
                {
                    buys = await db.Buys.Where(b => b.OrderId == item.Id).ToListAsync();
                    order = await db.Orders.FirstOrDefaultAsync(o => o.Id == item.Id);
                    if (order != null)
                    {
                        foreach (var buy in buys)
                        {
                            db.Buys.Remove(buy);
                        }
                        db.Orders.Remove(order);
                        await db.SaveChangesAsync();
                    }
                }
            }
            return RedirectToAction("AdministratorOrders");
        }

        public async Task<IActionResult> AdministratorOpenEditOrder(int? Id)
        {
            Order? order = await db.Orders.FirstOrDefaultAsync(o => o.Id == Id);
            return View("AdministratorEditOrder", order);
        }

        [HttpPost]
        public async Task<IActionResult> AdministratorEditOrder(int Id, bool isReady, bool isPaid)
        {
            Order? order = await db.Orders.FirstOrDefaultAsync(o => o.Id == Id);
            if (order != null)
            {
                order.isReady = isReady;
                order.isPaid = isPaid;
                db.Orders.Update(order);
                await db.SaveChangesAsync();
            }
            return RedirectToAction("AdministratorOrders"); 
        }
    }
}
