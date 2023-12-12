using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using PasswordManager.Models;
using PasswordManager.Repositories;
using PasswordManager.Utils;
using PasswordManager.ViewModels;

namespace PasswordManager.Controllers
{
	[Authorize]
	public class PasswordsController : Controller
    {
        private readonly MyPasswordsRepo passwordRepo;
        private readonly CategoriesRepo categoryRepo;
        private readonly IMapper mapper;
        private readonly IToastNotification toastNotification;

        public PasswordsController(MyPasswordsRepo passwordRepo, CategoriesRepo categoryRepo, IMapper mapper, IToastNotification toastNotification)
        {
            this.passwordRepo = passwordRepo;
            this.categoryRepo = categoryRepo;
            this.mapper = mapper;
            this.toastNotification = toastNotification;
        }

        public async Task<IActionResult> PasswordList()
        {
            var data = await passwordRepo.GetAllAsync().Where(x => x.UserID == "").Include(x => x.Categories).AsNoTracking().ToListAsync();
            var mapData = mapper.Map<List<MyPasswordWithCategory>>(data);

            return View(mapData);
        }

        public async Task<IActionResult> CreateNewPassword()
        {
            var data = await categoryRepo.GetAllAsync().Where(x => x.UserID == "").AsNoTracking().ToListAsync();
            ViewBag.Categories = mapper.Map<List<CategoriesViewModel>>(data);

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewPassword(MyPasswordsViewModel myPasswordsViewModel)
        {
            if (!ModelState.IsValid)
            {
                toastNotification.AddWarningToastMessage("Bilgiler Doğrulanamadı!");
                return RedirectToAction(nameof(CreateNewPassword));
            }

            await passwordRepo.CreateAsync(mapper.Map<MyPasswords>(myPasswordsViewModel));
            toastNotification.AddSuccessToastMessage("Yeni Şifre Kaydı Başarılı!");

            return RedirectToAction(nameof(PasswordList));
        }

        public async Task<IActionResult> UpdatePassword(int id)
        {
            var data = await passwordRepo.GetByIdAsync(id);
            var mapData = mapper.Map<MyPasswordsViewModel>(data);

            var categoryData = await categoryRepo.GetAllAsync().Where(x => x.UserID == "").AsNoTracking().ToListAsync();
            ViewBag.Categories = mapper.Map<List<CategoriesViewModel>>(categoryData);

            return View(mapData);
        }

        [HttpPost]
        public IActionResult UpdatePassword(MyPasswordsViewModel myPasswordsViewModel)
        {
            if (!ModelState.IsValid)
            {
                toastNotification.AddWarningToastMessage("Bilgiler Doğrulanamadı!");
                return RedirectToAction(nameof(UpdatePassword), new { id = myPasswordsViewModel.Id });
            }

            passwordRepo.Update(mapper.Map<MyPasswords>(myPasswordsViewModel));
            toastNotification.AddSuccessToastMessage("Şifre Kaydı Güncellendi!");

            return RedirectToAction(nameof(PasswordList));
        }

        [HttpPost]
        public async Task<IActionResult> DeletePassword(int id)
        {
            var data = await passwordRepo.GetByIdAsync(id);
            passwordRepo.Delete(data);

            toastNotification.AddSuccessToastMessage("Şifre Kaydı Silindi!");

            return RedirectToAction(nameof(PasswordList));
        }

        public JsonResult GeneratePassword()
        {
            try
            {
                var newPassword = PasswordGenerator.GeneratePassword();

                return Json(new
                {
                    success = true,
                    data = newPassword
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }
    }
}
