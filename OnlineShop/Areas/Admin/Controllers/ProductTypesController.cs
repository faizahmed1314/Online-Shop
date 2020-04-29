using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using OnlineShop.Data;
using OnlineShop.Models;

namespace OnlineShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ProductTypesController : Controller
    {
        private ApplicationDbContext _db;
        public ProductTypesController(ApplicationDbContext db)
        {
            _db = db;
        }
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View(_db.productTypes.ToList());
        }
        
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductTypes productTypes)
        {
            if (ModelState.IsValid)
            {
                _db.productTypes.Add(productTypes);
                await _db.SaveChangesAsync();
                TempData["save"] = "Product save successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(productTypes);
        }
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var productType = _db.productTypes.Find(id);
            if (productType == null)
            {
                return NotFound();
            }
            return View(productType);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductTypes productTypes)
        {
            if (ModelState.IsValid)
            {
                _db.Update(productTypes);
                await _db.SaveChangesAsync();
                TempData["Edit"] = "Successfully Updated!";
                return RedirectToAction(nameof(Index));
            }
            return View(productTypes);
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var productType = _db.productTypes.Find(id);
            if (productType == null)
            {
                return NotFound();
            }
            return View(productType);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public  IActionResult Details(ProductTypes productTypes)
        {
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var productType = _db.productTypes.Find(id);
            if (productType == null)
            {
                return NotFound();
            }
            return View(productType);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id,ProductTypes productTypes)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (id != productTypes.Id)
            {
                return NotFound();
            }
            var productType = _db.productTypes.Find(id);
            if (productType == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _db.Remove(productType);
                await _db.SaveChangesAsync();
                TempData["Delete"] = "Successfully Deleted!";
                return RedirectToAction(nameof(Index));
            }
            return View(productTypes);
        }
    }
}