using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OnlineShop.Data;
using OnlineShop.Models;
using OnlineShop.Utility;

namespace OnlineShop.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        
     
        private ApplicationDbContext _db;
       
        public HomeController(ApplicationDbContext db)
        {
            _db = db;
          
        }

        public IActionResult Index()
        {
            var data = _db.products.Include(x => x.productTypes).Include(x => x.specialTag).ToList();
            return View(data);
            
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var product = _db.products.Include(x => x.productTypes).Include(x => x.specialTag).FirstOrDefault(x => x.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        [ActionName("Details")]
        public IActionResult ProductDetails(int? id)
        {
            List<Product> products = new List<Product>();
            if (id == null)
            {
                return NotFound();
            }
            var product = _db.products.Include(x => x.productTypes).Include(x => x.specialTag).FirstOrDefault(x => x.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            //Check if there exist in product in the session 

            products = HttpContext.Session.Get<List<Product>>("products");
            if (products == null)
            {
                products = new List<Product>();
            }
            products.Add(product);

            //Add product to the session
            HttpContext.Session.Set("products", products);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public ActionResult Remove(int? id)
        {
            List<Product> products = new List<Product>();
            products = HttpContext.Session.Get<List<Product>>("products");
            if (products != null)
            {
                var p = products.FirstOrDefault(c => c.Id == id);
                if (p != null)
                {
                    products.Remove(p);
                    HttpContext.Session.Set("products", products);
                }
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
