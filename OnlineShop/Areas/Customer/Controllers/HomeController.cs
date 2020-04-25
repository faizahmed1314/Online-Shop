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
    }
}
