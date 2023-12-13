using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PasswordManager.Models;
using PasswordManager.Utils;
using PasswordManager.ViewModels;

namespace PasswordManager.Controllers
{
    public class SessionController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly PasswordResetMail emailService;

        public SessionController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, PasswordResetMail emailService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.emailService = emailService;
        }

        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel signInViewModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Email veya Şifre Yanlış");
                return View();
            }

            var user = await userManager.FindByEmailAsync(signInViewModel.Email);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Email veya Şifre Yanlış");
                return View();
            }

            var signInResult = await signInManager.PasswordSignInAsync(user, signInViewModel.Password, signInViewModel.RememberMe, true);

            if (signInResult.IsLockedOut)
            {
                ModelState.AddModelError(string.Empty, "3 dakika boyunca giriş yapamazsınız.");
                return View();
            }

            if (!signInResult.Succeeded)
            {
                ModelState.AddModelError(string.Empty, $"Başarısız giriş sayısı = {await userManager.GetAccessFailedCountAsync(user)}");
                return View();
            }

            return RedirectToAction("PasswordList", "Passwords");
        }

        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel signUpViewModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Eksik veya Hatalı Bilgi.");
                return View();
            }

            var result = await userManager.CreateAsync(new() { UserName = signUpViewModel.UserName, PhoneNumber = signUpViewModel.PhoneNumber, Email = signUpViewModel.Email, CreatedDate = DateTime.Now }, signUpViewModel.PasswordConfirm);

            if (!result.Succeeded)
            {
                foreach (IdentityError errors in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, errors.Description);
                }

                return View();
            }

            TempData["SuccessMessage"] = "Üye Kayıt İşlemi Tamamlandı. Lütfen Giriş Yapınız.";
            return RedirectToAction(nameof(SignIn));
        }

        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel forgetPasswordViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var user = await userManager.FindByEmailAsync(forgetPasswordViewModel.Email);

            if (user == null)
            {
                ModelState.AddModelError(String.Empty, "Bu email adresine sahip kullanıcı bulunamamıştır.");
                return View();
            }

            string token = await userManager.GeneratePasswordResetTokenAsync(user);

            var resetLink = Url.Action("ResetPassword", "Session", new { userId = user.Id, token }, HttpContext.Request.Scheme);

            await emailService.SendResetPasswordEmail(resetLink!, user.Email);

            TempData["SuccessMessage"] = "Şifre Yenileme E-Mail'i Gönderilmiştir.";
            return RedirectToAction(nameof(SignIn));
        }

        public IActionResult ResetPassword(string userId, string token)
        {
            TempData["userId"] = userId;
            TempData["token"] = token;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(PasswordChangeViewModel passwordChangeViewModel)
        {
            var userId = TempData["userId"];
            var token = TempData["token"];

            if (userId == null || token == null)
            {
                throw new Exception("Bir hata meydana geldi");
            }

            var user = await userManager.FindByIdAsync(userId.ToString()!);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı bulunamamıştır.");
                return View();
            }

            IdentityResult result = await userManager.ResetPasswordAsync(user, token.ToString()!, passwordChangeViewModel.ConfirmPassword);

            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Şifreniz Başaroyla Güncellenmiştir.";
                return RedirectToAction(nameof(SignIn));
            }
            else
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View();
        }
    }
}
