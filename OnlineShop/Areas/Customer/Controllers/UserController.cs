using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Data;
using OnlineShop.Models;

namespace OnlineShop.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class UserController : Controller
    {
        private UserManager<IdentityUser> _userManager;
        private ApplicationDbContext _db;

        public UserController(UserManager<IdentityUser> userManager, ApplicationDbContext db)
        {
            _userManager = userManager;
            _db = db;
        }
        public IActionResult Index()
        {
            var dd = _userManager.GetUserId(HttpContext.User);
            var dataList = _db.applicationUsers.ToList();
            return View(dataList);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ApplicationUser user)
        {
            if (ModelState.IsValid)
            {
                user.Email = user.UserName;
                var result = await _userManager.CreateAsync(user, user.PasswordHash);
                if (result.Succeeded)
                {
                    //setting user role by default as a normal user
                    var setRoleByDefault = await _userManager.AddToRoleAsync(user, "User");
                    TempData["save"] = "User create successfully";
                    return RedirectToAction(nameof(Index));
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View();
        }

        public IActionResult Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = _db.applicationUsers.FirstOrDefault(c => c.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ApplicationUser user)
        {
            var userInfo = _db.applicationUsers.FirstOrDefault(c => c.Id == user.Id);
            if (userInfo == null)
            {
                return NotFound();
            }

            userInfo.FirstName = user.FirstName;
            userInfo.LastName = user.LastName;



            var result = await _userManager.UpdateAsync(userInfo);
            if (result.Succeeded)
            {
                TempData["Edit"] = "User updated successfully";
                return RedirectToAction(nameof(Index));
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View();
        }

        public IActionResult Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = _db.applicationUsers.FirstOrDefault(c => c.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        public IActionResult Lockout(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = _db.applicationUsers.FirstOrDefault(c => c.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Lockout(ApplicationUser user)
        {
            var userInfo = _db.applicationUsers.FirstOrDefault(c => c.Id == user.Id);
            if (userInfo == null)
            {
                return NotFound();
            }

            userInfo.LockoutEnd = DateTime.Now.AddYears(10);
            var rowAffected = _db.SaveChanges();
            if (rowAffected > 0)
            {
                TempData["Delete"] = "User locked successfully";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public IActionResult Active(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = _db.applicationUsers.FirstOrDefault(c => c.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> Active(ApplicationUser user)
        {
            var userInfo = _db.applicationUsers.FirstOrDefault(c => c.Id == user.Id);
            if (userInfo == null)
            {
                return NotFound();
            }

            userInfo.LockoutEnd = DateTime.Now.AddDays(-1);
            var rowAffected = _db.SaveChanges();
            if (rowAffected > 0)
            {
                TempData["save"] = "User actived successfully";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public IActionResult Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = _db.applicationUsers.FirstOrDefault(c => c.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(ApplicationUser user)
        {
            var userInfo = _db.applicationUsers.FirstOrDefault(c => c.Id == user.Id);
            if (userInfo == null)
            {
                return NotFound();
            }

            _db.applicationUsers.Remove(userInfo);
            var rowAffected = _db.SaveChanges();
            if (rowAffected > 0)
            {
                TempData["Delete"] = "User deleted successfully";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
    }
}