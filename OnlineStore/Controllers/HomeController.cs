using Microsoft.AspNetCore.Mvc;
using OnlineStore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Diagnostics;
using Microsoft.AspNetCore.Identity;

namespace OnlineStore.Controllers
{
    public class HomeController : Controller
    {
        ApplicationContext db;
        public HomeController(ApplicationContext context)
        {
            this.db = context;
            ViewBag.categories = db.Categories.ToList();
        }

        
        public async Task<IActionResult> Index()
        {
            List<Product> productsSale = await db.Products.Include(p => p.imagesProduct).Where(p => p.Price != p.PriceSale).ToListAsync();
            return View(productsSale);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddToBasket(int Id)
        {
            Product? product = await db.Products.FirstOrDefaultAsync(p => p.Id == Id);
            if (product != null && User?.Identity?.Name != null)
            {
                Buy buy = new Buy();
                buy.Product = product;
                buy.Count = 1;
                buy.buyPrice = product.PriceSale;
                buy.buySum = buy.Count * buy.buyPrice;

                User? user = await db.Users.FirstOrDefaultAsync(u => u.Email == User.Identity.Name);

                // if() - если есть заказ в БД с нужным пользователем и заказ еще не зарегистрирован,
                // то добавляем покупку в него, если нет то создать новый заказ
                if (user != null)
                {
                    Order? order = await db.Orders.Include(o => o.User).Include(o=>o.products).FirstOrDefaultAsync(o => (o.User.Id == user.Id) && (o.isRegistered == false));
                    if (order != null && order.products != null)
                    {
                        Buy? checkBuy = await db.Buys.Include(b => b.Product).FirstOrDefaultAsync(b => (b.OrderId == order.Id) && (b.Product == buy.Product));
                        if (checkBuy != null)
                        {
                            checkBuy.Count++;
                            checkBuy.buySum = (buy.Count+1) * buy.buyPrice;
                            db.Buys.Update(checkBuy);
                            await db.SaveChangesAsync();
                        }
                        else
                        {
                            order.products.Add(buy);
                        }
                        order.CalcSumOfOrder();
                        db.Orders.Update(order);
                        await db.SaveChangesAsync();
                    }
                    else
                    {
                        db.Buys.Add(buy);
                        await db.SaveChangesAsync();

                        order = new Order();
                        order.DateOrder = DateTime.Now;
                        order.User = user;         
                        order.products = new List<Buy>();

                        order.isRegistered = false;
                        order.isReady = false;
                        order.isPaid = false;

                        order.products.Add(buy);
                        order.CalcSumOfOrder();

                        db.Orders.Add(order);
                        await db.SaveChangesAsync();
                    }
                }
            }
            return RedirectToAction("Index", "Home");
        }
    }
}