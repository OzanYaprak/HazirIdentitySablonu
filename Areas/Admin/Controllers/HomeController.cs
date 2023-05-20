using AspNetCoreIdentityApp.Areas.Admin.ViewModels;
using AspNetCoreIdentityApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace AspNetCoreIdentityApp.Areas.Admin.Controllers
{
    [Area("Admin")]    
    public class HomeController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public HomeController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [Authorize(Roles = "Admin")]
        [Route("/Admin")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [Route("/Admin/UserList")]
        public async Task<IActionResult> UserList()
        {
            var userList = await _userManager.Users.ToListAsync();
            var userViewModelList = userList.Select(a => new AdminUserViewModel()
            {
                UserID = a.Id,
                UserName = a.UserName,
                UserEmail = a.Email,
                PictureURL = a.Picture
            }).ToList();



            return View(userViewModelList);
        }
    }
}
