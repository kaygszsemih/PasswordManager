using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using PasswordManager.Models;
using PasswordManager.ViewModels;

namespace PasswordManager.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly IToastNotification toastNotification;
        private readonly IMapper mapper;

        public UserController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IToastNotification toastNotification, IMapper mapper)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.toastNotification = toastNotification;
            this.mapper = mapper;
        }

        public async Task<IActionResult> MyProfile()
        {
            var data = await userManager.GetUserAsync(User);
            var mapData = mapper.Map<UserInfoViewModel>(data);

            return View(mapData);
        }

        public async Task<IActionResult> EditProfile()
        {
            var data = await userManager.GetUserAsync(User);
            var mapData = mapper.Map<UserInfoViewModel>(data);

            return View(mapData);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(UserInfoViewModel userInfoViewModel)
        {
            var currentUser = await userManager.GetUserAsync(User);
            currentUser.UserName = userInfoViewModel.UserName;
            currentUser.PhoneNumber = userInfoViewModel.PhoneNumber;
            currentUser.Email = userInfoViewModel.Email;
            currentUser.UpdatedDate = DateTime.Now;

            var updateToUserResult = await userManager.UpdateAsync(currentUser);

            if (!updateToUserResult.Succeeded)
            {
                foreach (IdentityError error in updateToUserResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                    return View();
                }
            }

            await userManager.UpdateSecurityStampAsync(currentUser);
            await signInManager.SignOutAsync();

            TempData["SuccessMessage"] = "Üye Bilgileri Başarıyla Değiştirilmiştir.";
            return RedirectToAction(nameof(SignIn), "Session");
        }

        public IActionResult PasswordChange()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PasswordChange(PasswordChangeViewModel passwordChangeViewModel)
        {

            if (!ModelState.IsValid)
            {
                toastNotification.AddWarningToastMessage("Bilgiler Doğrulanamadı!");
                return RedirectToAction(nameof(MyProfile));
            }

            var currentUser = await userManager.GetUserAsync(User);


            var checkOldPassword = await userManager.CheckPasswordAsync(currentUser, passwordChangeViewModel.OldPassword);

            if (!checkOldPassword)
            {
                ModelState.AddModelError(string.Empty, "Eski şifreniz yanlış");
                return View();
            }

            var resultChangePassword = await userManager.ChangePasswordAsync(currentUser, passwordChangeViewModel.OldPassword, passwordChangeViewModel.ConfirmPassword);

            if (!resultChangePassword.Succeeded)
            {
                foreach (IdentityError error in resultChangePassword.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                    return View();
                }
            }

            currentUser.UpdatedDate = DateTime.Now;
            await userManager.UpdateAsync(currentUser);
            await userManager.UpdateSecurityStampAsync(currentUser);
            await signInManager.SignOutAsync();
            await signInManager.PasswordSignInAsync(currentUser, passwordChangeViewModel.ConfirmPassword, false, false);

            TempData["SuccessMessage"] = "Şifreniz Başarıyla Değiştirilmiştir.";
            return RedirectToAction(nameof(SignIn), "Session");
        }

        public new async Task<IActionResult> SignOut()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction(nameof(SignIn), "Session");
        }
    }
}
