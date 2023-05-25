using AspNetCoreIdentityApp.Models;
using AspNetCoreIdentityApp.Core.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using AspNetCoreIdentityApp.Extensions;
using AspNetCoreIdentityApp.Services;
using System.Security.Claims;
using AspNetCoreIdentityApp.Core.Models;

namespace AspNetCoreIdentityApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailService _emailService;


        public HomeController(ILogger<HomeController> logger, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IEmailService emailService)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
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

            if (!identityResult.Succeeded)
            {
                //ModelStateExtensions kısmından gelen kod satırı
                ModelState.AddModelErrorList(identityResult.Errors.Select(a => a.Description).ToList());

                return View();
            }

            if (identityResult.Succeeded)
            {
                //örneğin bir borsa bilgisi için claim, oluşturulduğu günden itibaren 10 gün erişebilinsin.
                var exchangeExpireClaim = new Claim("ExchangeExpireDate", DateTime.Now.AddDays(10).ToString());

                //üye olan kullanıcıyı burada yakalıyoruz.
                var user = await _userManager.FindByNameAsync(request.UserName);

                //üye olan kullanıcıya(user), yazmış olduğumuz claim'i ekliyoruz(exchangeExpireClaim)
                var claimresult = await _userManager.AddClaimAsync(user, exchangeExpireClaim);


                if (!claimresult.Succeeded)
                {
                    ModelState.AddModelErrorList(identityResult.Errors.Select(a => a.Description).ToList());

                    return View();
                }


                TempData["SignUpSucceedMessage"] = "Üyelik işleminiz gerçekleşmiştir.";

                return RedirectToAction(nameof(HomeController.SignUp));
            }

            //ModelStateExtensions kısmından gelen kod satırı
            ModelState.AddModelErrorList(identityResult.Errors.Select(a => a.Description).ToList()); //??Hata verebilir


            return View(); //??Hata verebilir
        }





        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel model, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            returnUrl ??= Url.Action("Index", "Home");

            //giriş yapan kullanıcıyı email bilgisinden yakalıyoruz.
            var loginUser = await _userManager.FindByEmailAsync(model.Email);

            if (loginUser == null)
            {
                ModelState.AddModelError(string.Empty, "Email veya şifreniz yanlış");
                return View();
            }

            //şifre kontrolü yap
            var passwordcheck = await _userManager.CheckPasswordAsync(loginUser, model.Password);

            //eğer şifre doğru değil ise / burada sadece şifre yanlış demememizin sebebi kötü niyetli birinin email doğru fakat şifresi yanlış dememesi için.
            if (!passwordcheck)
            {
                ModelState.AddModelError(string.Empty, "Email veya şifreniz yanlış");
                return View();
            }

            //login/signin sonucunu yakalıyoruz
            var signInResult = await _signInManager.PasswordSignInAsync(loginUser, model.Password, model.RememberMe, true);

            //eğer birden fazla yanlış giriş denemesi yapıldı ise..
            if (signInResult.IsLockedOut)
            {
                ModelState.AddModelErrorList(new List<string>() { "Çok fazla yanlış giriş denemesi yaptınız, 3 dk sonra tekrar deneyiniz." });
                return View();
            }

            if (!signInResult.Succeeded)
            {
                //ModelStateExtensions kısmından gelen kod satırı
                ModelState.AddModelErrorList(new List<string>() { "Email veya şifreniz yanlış.", $"Başarısız giriş denemesi 5/{await _userManager.GetAccessFailedCountAsync(loginUser)}" });
                return View();
            }

            return Redirect(returnUrl);
        }







        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel request)
        {
            var loginUser = await _userManager.FindByEmailAsync(request.Email);

            if (loginUser == null)
            {
                ModelState.AddModelError(string.Empty, "Bu email adresine sahip bir kullanıcı bulunamamıştır.");
                return View();
            }

            string passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(loginUser);

            var passwordResetLink = Url.Action("ResetPassword", "Home", new
            {
                userID = loginUser.Id,
                passwordResetToken = passwordResetToken
            }, HttpContext.Request.Scheme);


            await _emailService.SendPasswordResetEmail(passwordResetLink, loginUser.Email);



            TempData["PasswordResetSuccess"] = "Şifre yenileme bağlantısı tarafınıza mail olarak gönderilmiştir.";

            return RedirectToAction(nameof(ForgotPassword));
        }








        [HttpGet]
        public IActionResult ResetPassword(string userID, string passwordResetToken)
        {
            TempData["userID"] = userID;
            TempData["passwordResetToken"] = passwordResetToken;


            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel request)
        {
            var userID = TempData["userID"];
            var passwordResetToken = TempData["passwordResetToken"];

            if (userID == null || passwordResetToken == null)
            {
                throw new Exception("Bir hata meydana geldi.");
            }

            var loginUser = await _userManager.FindByIdAsync(userID.ToString());

            if (loginUser == null)
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı Bulunamamıştır.");
                return View();
            }

            var result = await _userManager.ResetPasswordAsync(loginUser, passwordResetToken.ToString(), request.Password);

            if (result.Succeeded)
            {
                TempData["PasswordResetSucceedMessage"] = "Şifreniz başarıyla yenilenmiştir.";
            }
            else
            {
                ModelState.AddModelErrorList(result.Errors.Select(a => a.Description).ToList());
            }

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