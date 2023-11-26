using AspNetCoreIdentityApp.Web.DersIcerigi.Models;
using AspNetCoreIdentityApp.Web.DersIcerigi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreIdentityApp.Web.DersIcerigi.Controllers
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

            var currentUser =await _userManager.FindByNameAsync(User.Identity!.Name!);
            var userViewMoodel = new UserViewModel
            {
                Email = currentUser!.Email,
                UserName = currentUser!.UserName,
                PhoneNumber = currentUser!.PhoneNumber,
            };

             
            return View(userViewMoodel);
        }

        public async Task LogOut()
        {

            await _signInManager.SignOutAsync();

            
        }
    }
}
