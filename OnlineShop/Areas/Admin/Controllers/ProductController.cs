using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models;

namespace OnlineShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ProductController : Controller
    {
        private ApplicationDbContext _db;
        private IHostingEnvironment _he;

        public ProductController(ApplicationDbContext db, IHostingEnvironment he)
        {
            _db = db;
            _he = he;
        }

        public IActionResult Index()
        {
            var data = _db.products.Include(x => x.productTypes).Include(x => x.specialTag).ToList();
            return View(data);
        }
        [HttpPost]
        public IActionResult Index(decimal? minPrice, decimal? maxPrice)
        {
            var data = _db.products.Include(x => x.productTypes).Include(x => x.specialTag).Where(c=>c.Price>=minPrice&& c.Price<=maxPrice).ToList();
            if(minPrice==null || maxPrice == null)
            {
                data = _db.products.Include(x => x.productTypes).Include(x => x.specialTag).ToList();

            }
            return View(data);
        }
        public IActionResult Create()
        {
            var productData = _db.productTypes.ToList();
            var tagData = _db.specialTags.ToList();
            ViewBag.ProductTypeId = new SelectList(productData, "Id", "ProductType");
            ViewBag.TagId = new SelectList(tagData, "Id", "Tag");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                var checkName = _db.products.Where(c => c.ProductName == product.ProductName).SingleOrDefault();
                if (checkName != null)
                {
                    ViewBag.Message = "The name is already exist!";
                    var productData = _db.productTypes.ToList();
                    var tagData = _db.specialTags.ToList();
                    ViewBag.ProductTypeId = new SelectList(productData, "Id", "ProductType");
                    ViewBag.TagId = new SelectList(tagData, "Id", "Tag");
                    return View(product);
                }
                if (image != null)
                {
                    //Getting the path from the directory and get the name of image as his own file name
                    var name = Path.Combine(_he.WebRootPath + "/Images", Path.GetFileName(image.FileName));
                    //copy the image to the folder/ FileStream a er path dite hoi and mode bole dite hoi
                    await image.CopyToAsync(new FileStream(name, FileMode.Create));
                    //declare the product image property as the folder + image file name
                    product.Image = "Images/" + image.FileName;
                }
                if (image == null)
                {
                    product.Image = "Images/default-image.jpg";
                }
                _db.products.Add(product);
                await _db.SaveChangesAsync();
                TempData["save"] = "Product save successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        public IActionResult Edit(int? id)
        {
            var productData = _db.productTypes.ToList();
            var tagData = _db.specialTags.ToList();
            ViewBag.ProductTypeId = new SelectList(productData, "Id", "ProductType");
            ViewBag.TagId = new SelectList(tagData, "Id", "Tag");

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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Product product, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    //Getting the path from the directory and get the name of image as his own file name
                    var name = Path.Combine(_he.WebRootPath + "/Images", Path.GetFileName(image.FileName));
                    //copy the image to the folder/ FileStream a er path dite hoi and mode bole dite hoi
                    await image.CopyToAsync(new FileStream(name, FileMode.Create));
                    //declare the product image property as the folder + image file name
                    product.Image = "Images/" + image.FileName;
                }
                if (image == null)
                {
                    product.Image = "Images/default-image.jpg";
                }
                _db.products.Update(product);
                await _db.SaveChangesAsync();
                TempData["save"] = "Product save successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(product);
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
        [ValidateAntiForgeryToken]
        public IActionResult Details(Product product)
        {
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int? id)
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
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _db.products.Where(c => c.Id == id).FirstOrDefault();
            if (product == null)
            {
                return NotFound();
            }

            _db.Remove(product);
            await _db.SaveChangesAsync();
            TempData["Delete"] = "Successfully Deleted!";
            return RedirectToAction(nameof(Index));

        }
    }
}