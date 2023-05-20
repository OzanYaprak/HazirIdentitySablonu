using AspNetCoreIdentityApp.Areas.Admin.ViewModels;
using AspNetCoreIdentityApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AspNetCoreIdentityApp.Extensions;
using Microsoft.EntityFrameworkCore;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;

namespace AspNetCoreIdentityApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RolesController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        public RolesController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var roles = await _roleManager.Roles.Select(a => new RoleViewModel()
            {
                RoleID = a.Id,
                RoleName = a.Name
            }).ToListAsync();

            return View(roles);
        }






        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel request)
        {
            var result = await _roleManager.CreateAsync(new AppRole() { Name = request.RoleName });

            if (!result.Succeeded)
            {
                ModelState.AddModelErrorList(result.Errors);
                return View();
            }

            TempData["SucceededMessage"] = "Ünvan Başarıyla Eklenmiştir.";

            return RedirectToAction(nameof(RolesController.Index));
        }








        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> UpdateRole(string id)
        {
            var roleToUpdate = await _roleManager.FindByIdAsync(id);

            if (roleToUpdate == null)
            {
                throw new Exception("Ünvan Bulunamamıştır");
            }


            return View(new UpdateRoleViewModel()
            {
                RoleID = roleToUpdate.Id,
                RoleName = roleToUpdate.Name
            });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> UpdateRole(UpdateRoleViewModel request)
        {
            var roleToUpdate = await _roleManager.FindByIdAsync(request.RoleID);

            if (roleToUpdate == null)
            {
                throw new Exception("Ünvan Bulunamamıştır");
            }

            roleToUpdate.Name = request.RoleName;

            await _roleManager.UpdateAsync(roleToUpdate);

            TempData["SucceededMessage"] = "Ünvan Başarıyla Güncellenmiştir.";

            return View();
        }






        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var roleToDelete = await _roleManager.FindByIdAsync(id);


            if (roleToDelete == null)
            {
                throw new Exception("Ünvan Bulunamamıştır");
            }

           var result = await _roleManager.DeleteAsync(roleToDelete);

            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.Select(a => a.Description).First());
            }

            TempData["SucceededMessage"] = "Ünvan Başarıyla Silinmiştir.";

            return RedirectToAction(nameof(RolesController.Index));
        }





        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> AssingRole(string id)
        {
            var currentUser = await _userManager.FindByIdAsync(id);
            ViewBag.userid = id;

            var roles = await _roleManager.Roles.ToListAsync();
            var roleViewModelList = new List<AssingRoleViewModel>();
            var userRoles = await _userManager.GetRolesAsync(currentUser);

            foreach (var role in roles)
            {
                var assingRoleViewModel = new AssingRoleViewModel() 
                {
                    RoleID = role.Id,
                    RoleName = role.Name
                };


                if (userRoles.Contains(role.Name))
                {
                    assingRoleViewModel.Exist = true;
                }

                roleViewModelList.Add(assingRoleViewModel);
            }

            
            return View(roleViewModelList);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AssingRole(string id ,List<AssingRoleViewModel> requestList)
        {
            var user = await _userManager.FindByIdAsync(id);

            foreach (var role in requestList)
            {
                if (role.Exist)
                {
                    await _userManager.AddToRoleAsync(user, role.RoleName);
                }
                else
                {
                    await _userManager.RemoveFromRoleAsync(user, role.RoleName);
                }
            }


            return RedirectToAction(nameof(HomeController.UserList),"Home");
        }
    }
}