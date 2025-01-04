using Demo.Application.Common.Interfaces;
using Demo.Domain.Entities;
using Demo.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(
            IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManage,
            RoleManager<IdentityRole> roleManager,
            SignInManager<ApplicationUser> signInManager
            )
        {
            _unitOfWork = unitOfWork;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _userManager = userManage;
        }

        public IActionResult Login(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            LoginViewModel loginViewModel = new LoginViewModel() 
            {
                RedirectUrl = returnUrl,
            };
            return View(loginViewModel);
        }
        public IActionResult Register()
        {
            return View();
        }
    }
}
