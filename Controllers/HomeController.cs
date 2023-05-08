using AspNetCoreIdentityApp.Models;
using AspNetCoreIdentityApp.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using AspNetCoreIdentityApp.Extensions;

namespace AspNetCoreIdentityApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;



        public HomeController(ILogger<HomeController> logger, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }






        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel request)
        {

            if (!ModelState.IsValid)
            {
                return View();
            }

            //KULLANICI YARATMA METODU
            var identityResult = await _userManager.CreateAsync(new AppUser { UserName = request.UserName, Email = request.Email, PhoneNumber = request.Phone }, request.Password);

            if (identityResult.Succeeded)
            {
                TempData["SignUpSucceedMessage"] = "Üyelik işleminiz gerçekleşmiştir.";

                return RedirectToAction(nameof(HomeController.SignUp));
            }

            //ModelStateExtensions kısmından gelen kod satırı
            ModelState.AddModelErrorList(identityResult.Errors.Select(a => a.Description).ToList());


            return View();
        }





        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel model, string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Action("Index", "Home");

            var loginUser = await _userManager.FindByEmailAsync(model.Email);

            if (loginUser == null)
            {
                ModelState.AddModelError(string.Empty, "Email veya şifreniz yanlış");
                return View();
            }

            var signInResult = await _signInManager.PasswordSignInAsync(loginUser, model.Password, model.RememberMe, true);

            if (signInResult.Succeeded)
            {
                return Redirect(returnUrl);
            }

            if (signInResult.IsLockedOut)
            {
                ModelState.AddModelErrorList(new List<string>() { "Çok fazla yanlış giriş denemesi yaptınız, 3 dk sonra tekrar deneyiniz." });
                return View();
            }

            //ModelStateExtensions kısmından gelen kod satırı
            ModelState.AddModelErrorList(new List<string>() { "Email veya şifreniz yanlış.", $"Başarısız giriş denemesi 5/{await _userManager.GetAccessFailedCountAsync(loginUser)}" });

            return View();
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