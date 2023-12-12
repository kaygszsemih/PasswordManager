using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using PasswordManager.Models;
using PasswordManager.Utils;

namespace PasswordManager.Controllers
{
	[Authorize]
	public class UserController : Controller
    {
		private readonly UserManager<AppUser> userManager;
		private readonly SignInManager<AppUser> signInManager;
		private readonly IToastNotification toastNotification;

		public UserController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IToastNotification toastNotification)
		{
			this.userManager = userManager;
			this.signInManager = signInManager;
			this.toastNotification = toastNotification;
		}

		public IActionResult Index()
        {
            return View();
        }

		public async Task<IActionResult> SignOut()
		{
			await signInManager.SignOutAsync();
			return RedirectToAction(nameof(SessionController.SignIn));
		}
	}
}
