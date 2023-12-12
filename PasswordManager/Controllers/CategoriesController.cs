using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using PasswordManager.Models;
using PasswordManager.Repositories;
using PasswordManager.ViewModels;

namespace PasswordManager.Controllers
{
	[Authorize]
	public class CategoriesController : Controller
    {
        private readonly CategoriesRepo categoryRepo;
        private readonly IMapper mapper;
        private readonly IToastNotification toastNotification;

        public CategoriesController(CategoriesRepo categoryRepo, IMapper mapper, IToastNotification toastNotification)
        {
            this.categoryRepo = categoryRepo;
            this.mapper = mapper;
            this.toastNotification = toastNotification;
        }

        public async Task<IActionResult> CategoryList()
        {
            var data = await categoryRepo.GetAllAsync().Where(x => x.UserID == "").AsNoTracking().ToListAsync();
            var mapData = mapper.Map<List<CategoriesViewModel>>(data);

            return View(mapData);
        }

        public IActionResult CreateNewCategory()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewCategory(CategoriesViewModel categoriesViewModel)
        {
            if (!ModelState.IsValid)
            {
                toastNotification.AddWarningToastMessage("Bilgiler Doğrulanamadı!");
                return RedirectToAction(nameof(CreateNewCategory));
            }

            await categoryRepo.CreateAsync(mapper.Map<Categories>(categoriesViewModel));
            toastNotification.AddSuccessToastMessage("Yeni Kategori Kaydı Başarılı!");

            return RedirectToAction(nameof(CategoryList));
        }

        public async Task<IActionResult> UpdateCategory(int id)
        {
            var data = await categoryRepo.GetByIdAsync(id);
            var mapData = mapper.Map<CategoriesViewModel>(data);

            return View(mapData);
        }

        [HttpPost]
        public IActionResult UpdateCategory(CategoriesViewModel categoriesViewModel)
        {
            if (!ModelState.IsValid)
            {
                toastNotification.AddWarningToastMessage("Bilgiler Doğrulanamadı!");
                return RedirectToAction(nameof(UpdateCategory), new { id = categoriesViewModel.Id });
            }

            categoryRepo.Update(mapper.Map<Categories>(categoriesViewModel));
            toastNotification.AddSuccessToastMessage("Kategori Güncellendi");

            return RedirectToAction(nameof(CategoryList));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var data = await categoryRepo.GetByIdAsync(id);
            categoryRepo.Delete(data);

            toastNotification.AddSuccessToastMessage("Kategori Silindi!");

            return RedirectToAction(nameof(CategoryList));
        }
    }
}
