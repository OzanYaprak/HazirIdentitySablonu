using AspNetCoreIdentityApp.Extensions;
using AspNetCoreIdentityApp.Models;
using AspNetCoreIdentityApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AspNetCoreIdentityApp.Controllers
{
    [Authorize]
    public class MemberController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;

        public MemberController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);

            var userViewModel = new UserViewModel
            {
                UserName = currentUser!.UserName,
                Email = currentUser!.Email,
                PhoneNumber = currentUser!.PhoneNumber
            };

            return View(userViewModel);
        }







        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }







        [HttpGet]
        public IActionResult PasswordChange()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PasswordChange(PasswordChangeViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
            var checkOldPassword = await _userManager.CheckPasswordAsync(currentUser, request.OldPassword);

            if (checkOldPassword == false)
            {
                ModelState.AddModelError(string.Empty, "Eski şifrenizi yanlış girdiniz.");
                return View();
            }

            if (request.OldPassword == request.NewPassword)
            {
                ModelState.AddModelError(string.Empty, "Yeni şifreniz eski şifrenizle aynı olamaz.");
                return View();
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(currentUser, request.OldPassword, request.NewPassword);

            if (!changePasswordResult.Succeeded)
            {
                ModelState.AddModelErrorList(changePasswordResult.Errors.Select(a => a.Description).ToList());
                return View();
            }

            //Şifre değiştirdikten sonra cookie yenilenmesi için kullanıcıya girdi çıktı yaptırıyoruz.
            await _userManager.UpdateSecurityStampAsync(currentUser);
            await _signInManager.SignOutAsync();
            await _signInManager.PasswordSignInAsync(currentUser, request.NewPassword, true, true);


            TempData["PasswordChangeSucceedMessage"] = "Şifreniz Başarıyla Güncellenmiştir.";


            return View();
        }






        [HttpGet]
        public async Task<IActionResult> UserEdit()
        {
            ViewBag.genderList = new SelectList(Enum.GetNames(typeof(Gender)));

            var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
            var userEditViewModel = new UserEditViewModel()
            {
                UserName = currentUser.UserName,
                Email = currentUser.Email,
                Phone = currentUser.PhoneNumber,
                City = currentUser.City,
                DateOfBirth = currentUser.DateOfBirth,
                Gender = currentUser.Gender
            };

            return View(userEditViewModel);
        }

    }
}
