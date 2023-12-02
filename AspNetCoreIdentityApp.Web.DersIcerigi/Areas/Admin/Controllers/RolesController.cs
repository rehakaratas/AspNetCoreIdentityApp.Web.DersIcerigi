using AspNetCoreIdentityApp.Web.DersIcerigi.Areas.Admin.Models;
using AspNetCoreIdentityApp.Web.DersIcerigi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreIdentityApp.Web.DersIcerigi.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class RolesController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        private readonly RoleManager<AppRole> _roleManager;

        public RolesController(RoleManager<AppRole> roleManager, UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult RoleCreate()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RoleCreate(RoleCreateViewModel request)
        {
            return View();
        }
    }
}
