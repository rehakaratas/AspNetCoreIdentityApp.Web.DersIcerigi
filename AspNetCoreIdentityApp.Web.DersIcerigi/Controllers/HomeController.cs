using AspNetCoreIdentityApp.Web.DersIcerigi.Models;
using AspNetCoreIdentityApp.Web.DersIcerigi.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using AspNetCoreIdentityApp.Web.DersIcerigi.Extensions;

namespace AspNetCoreIdentityApp.Web.DersIcerigi.Controllers
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

        public IActionResult Privacy()
        {
            return View();
        }


        public IActionResult SignUp()
        {



            return View();
        }


        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel model, string? returnUrl=null)
        {
            returnUrl = returnUrl ?? Url.Action("Index","Home");

            var hasUser = await _userManager.FindByEmailAsync(model.Email);
            if (hasUser == null)
            {
                ModelState.AddModelError(string.Empty, "Email veya Şifre yanlış");
                return View();
            }

            var signInResult = await _signInManager.PasswordSignInAsync(hasUser, model.Password,model.RememberMe,true);

            if(signInResult.Succeeded)
            {
                return Redirect(returnUrl);
            }

            if (signInResult.IsLockedOut)
            {
                ModelState.AddModelErrorList(new List<string>() { "3 dakika boyunca giriş yapamazsınız." });
                return View();
            }

            ModelState.AddModelErrorList(new List<string>() { $"Email veya Şifre yanlış",$"Başarısız giriş sayısı ={await _userManager.GetAccessFailedCountAsync(hasUser)}" });


            return View();

        }







        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel request)
        {

            if (!ModelState.IsValid)
            {
                return View();
            }

            var identityResult=await _userManager.CreateAsync(new() { UserName=request.UserName,PhoneNumber=request.Phone,Email=request.Email},request.Password);


            

            if (identityResult.Succeeded)
            {
                TempData["SuccessMessage"] = "Üyelik Kayıt İşlemi Başarıyla Gerçekleşmiştir";

                return RedirectToAction(nameof(HomeController.SignUp));
            }


            ModelState.AddModelErrorList(identityResult.Errors.Select(x=>x.Description).ToList());
            


            return View();
        }

        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel request)
        {
            var hasUser = await _userManager.FindByEmailAsync(request.Email);

            if (hasUser == null)
            {
                ModelState.AddModelError(String.Empty, "Bu email adresine sahip kullanıcı bulunamamıştır");
                return View();
            }

            string passwordResetToken =await _userManager.GeneratePasswordResetTokenAsync(hasUser);
            
            var passwordResetLink= Url.Action("ResetPassword","Home", new {userId=hasUser.Id,Token=passwordResetToken});

            TempData["SuccessMessage"] = "Şifre yenileme linki eposta adresinize gönderiliştir.";

            return RedirectToAction(nameof(ForgetPassword));

        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}