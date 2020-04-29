using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineShop.Areas.Admin.Models;
using OnlineShop.Data;

namespace OnlineShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoleController : Controller
    {
        RoleManager<IdentityRole> _roleManager;
        UserManager<IdentityUser> _userManager;
        ApplicationDbContext _db;
        public RoleController(RoleManager<IdentityRole> roleManager,ApplicationDbContext db,UserManager<IdentityUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _db = db;
        }
        public IActionResult Index()
        {
            var roles = _roleManager.Roles.ToList();
            ViewBag.Roles = roles;
            return View();
        }
        public IActionResult Create()
        {
            
            return View();
        }
        [HttpPost]
        public async Task< IActionResult> Create(string name)
        {
            IdentityRole role = new IdentityRole();
            role.Name = name;
            var isExist =await _roleManager.RoleExistsAsync(role.Name);
            if (isExist)
            {
                ViewBag.Message = "This name is already exist!";
                ViewBag.name = name;
                return View();
            }
            var result = await _roleManager.CreateAsync(role);
            if (result.Succeeded)
            {
                TempData["save"] = "Role created successfully";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public async Task< IActionResult> Edit(string id)
        {
            var role =await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            ViewBag.id = role.Id;
            ViewBag.name = role.Name;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Edit(string id, string name)
        {
            var role =await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            role.Name = name;
            
            var isExist = await _roleManager.RoleExistsAsync(role.Name);
            if (isExist)
            {
                ViewBag.Message = "This name is already exist!";
                ViewBag.name = name;
                return View();
            }
            var result = await _roleManager.UpdateAsync(role);
            if (result.Succeeded)
            {
                TempData["save"] = "Role updated successfully";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        public async Task<IActionResult> Delete(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            ViewBag.id = role.Id;
            ViewBag.name = role.Name;
            return View();
        }
        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            

            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                TempData["Delete"] = "Role deleted successfully";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public IActionResult Assign()
        {
            //get all userlist which are active or filter the inactive user
            var userlist = _db.Users.Where(c=>c.LockoutEnd<DateTime.Now||c.LockoutEnd==null).ToList();
            ViewBag.UserId = new SelectList(userlist, "Id", "UserName");
            var rolelist = _roleManager.Roles.ToList();
            ViewBag.RoleId = new SelectList(rolelist, "Name", "Name");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Assign(UserRoleVm userRole)
        {
            var user = _db.applicationUsers.FirstOrDefault(c => c.Id == userRole.UserId);
            var checkIsRoleAssigned =await _userManager.IsInRoleAsync(user, userRole.RoleId);
            if (checkIsRoleAssigned)
            {

                ViewBag.Message = "This user role is already assigned";
                var userlist = _db.Users.Where(c => c.LockoutEnd < DateTime.Now || c.LockoutEnd == null).ToList();
                ViewBag.UserId = new SelectList(userlist, "Id", "UserName");
                var rolelist = _roleManager.Roles.ToList();
                ViewBag.RoleId = new SelectList(rolelist, "Name", "Name");
                return View();
            }
            var role = await _userManager.AddToRoleAsync(user, userRole.RoleId);
            if (role.Succeeded)
            {
                TempData["save"] = "Role assigned successfully";
                return RedirectToAction(nameof(AssignUserRole));
            }
            return View();
        }

        public IActionResult AssignUserRole()
        {

            //using linq for getting user role value from AspNetUserRole table
            var result = from ur in _db.UserRoles
                         join r in _db.Roles on ur.RoleId equals r.Id
                         join u in _db.applicationUsers on ur.UserId equals u.Id
                         select new UserRoleMapping
                         {
                             UserId = ur.UserId,
                             RoleId=ur.RoleId,
                             UserName=u.UserName,
                             RoleName=r.Name
                         };
            return View(result.ToList());
        }
    }
}