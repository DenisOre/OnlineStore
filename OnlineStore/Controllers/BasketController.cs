using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Models;
using Microsoft.AspNetCore.Authorization;




namespace OnlineStore.Controllers
{
    [Authorize]
    public class BasketController : Controller
    {
        ApplicationContext db;
        public BasketController(ApplicationContext context)
        {
            this.db = context;
        }
        public async Task<IActionResult> Basket()
        {
            User? user = await db.Users.FirstOrDefaultAsync(u => u.Email == User.Identity.Name);
            Order? order = await db.Orders.Include(o => o.User).Include(o => o.products).Include(o => o.products).FirstOrDefaultAsync(o => (o.User.Id == user.Id) && (o.isRegistered == false));

            if (order == null)
            {
                ViewBag.BasketInfo = "Корзина пуста";
                return View("BasketInfo");
            }
            List<Buy> buys = await db.Buys.Include(b => b.Product).Where(b => b.OrderId == order.Id).ToListAsync();
            order.products = buys;

            return View(order);
        }


        public async Task<IActionResult> DeleteBuyFromOrder(int? Id)
        {
            Buy? delBuy = await db.Buys.Where(b => b.Id == Id).FirstOrDefaultAsync();
            if (delBuy != null)
            {
                Order? order = await db.Orders.FirstOrDefaultAsync(o => o.Id == delBuy.OrderId);
                db.Buys.Remove(delBuy);
                await db.SaveChangesAsync();
                if (order != null)
                {
                    order.products = await db.Buys.Where(b => b.OrderId == order.Id).ToListAsync();
                    order.CalcSumOfOrder();
                    db.Orders.Update(order);
                    await db.SaveChangesAsync();
                }
            }
            return RedirectToAction("Basket");
        }

        [HttpPost]
        public async Task<IActionResult> DecreaseCountOfProduct(int? Id)
        {
            Buy? buy = await db.Buys.FirstOrDefaultAsync(b => b.Id == Id);
            if (buy != null && buy.Count > 1)
            {
                buy.Count--;
                buy.CalcBuySum();
                db.Buys.Update(buy);
                await db.SaveChangesAsync();
                Order? order = await db.Orders.FirstOrDefaultAsync(o => o.Id == buy.OrderId);
                if (order != null)
                {
                    order.products = await db.Buys.Where(b => b.OrderId == order.Id).ToListAsync();
                    order.CalcSumOfOrder();
                    db.Orders.Update(order);
                    await db.SaveChangesAsync();
                }
            }
            return RedirectToAction("Basket");
        }

        [HttpPost]
        public async Task<IActionResult> IncreaseCountOfProduct(int? Id)
        {
            Buy? buy = await db.Buys.FirstOrDefaultAsync(b => b.Id == Id);
            if (buy != null)
            {
                buy.Count++;
                buy.CalcBuySum();
                db.Buys.Update(buy);
                await db.SaveChangesAsync();
                Order? order = await db.Orders.FirstOrDefaultAsync(o => o.Id == buy.OrderId);
                if (order != null)
                {
                    order.products = await db.Buys.Where(b => b.OrderId == order.Id).ToListAsync();
                    order.CalcSumOfOrder();
                    db.Orders.Update(order);
                    await db.SaveChangesAsync();
                }
            }
            return RedirectToAction("Basket");
        }

        public async Task<IActionResult> RegisterOrder(int? Id)
        {
            Order? order = await db.Orders.FirstOrDefaultAsync(o => o.Id == Id);
            if (order != null)
            {
                order.isRegistered = true;
                db.Orders.Update(order);
                await db.SaveChangesAsync();
                ViewBag.BasketInfo = $"Ваш заказ N {order.Id} оформлен!!!";
            }
            else 
            {
                ViewBag.BasketInfo = "Ошибка!!! Заказ не оформлен!!!";
            }
            return View("BasketInfo");
        }
    }
}
