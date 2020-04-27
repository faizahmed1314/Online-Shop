using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Data;
using OnlineShop.Models;
using OnlineShop.Utility;

namespace OnlineShop.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class OrderController : Controller
    {
        private ApplicationDbContext _db;
        public OrderController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Checkout()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Checkout(Order order)
        {
            order.OrderNo = GetOrderNo();
            order.OrderDate = DateTime.Now;
            List<Product> products = HttpContext.Session.Get<List<Product>>("products");
            if (products != null)
            {
                foreach(var item in products)
                {
                    OrderDetails orderDetails = new OrderDetails();
                    orderDetails.ProductId = item.Id;
                    order.OrderDetails.Add(orderDetails);
                }
            }
            _db.orders.Add(order);
            await _db.SaveChangesAsync();
            HttpContext.Session.Set("products", new List<Product>());
            return View();
        }

        public string GetOrderNo()
        {
            int rowCount = _db.orders.ToList().Count();
            return "000"+rowCount.ToString();
        }
    }
}