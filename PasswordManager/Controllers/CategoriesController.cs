using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using PasswordManager.Filters;
using PasswordManager.Models;
using PasswordManager.Repositories;
using PasswordManager.ViewModels;

namespace PasswordManager.Controllers
{
    [Authorize]
    public class CategoriesController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly CategoriesRepo categoryRepo;
        private readonly IMapper mapper;
        private readonly IToastNotification toastNotification;

        public CategoriesController(CategoriesRepo categoryRepo, IMapper mapper, IToastNotification toastNotification, UserManager<AppUser> userManager)
        {
            this.categoryRepo = categoryRepo;
            this.mapper = mapper;
            this.toastNotification = toastNotification;
            this.userManager = userManager;
        }

        public async Task<IActionResult> CategoryList()
        {
            var currentUser = await userManager.GetUserAsync(User);
            var data = await categoryRepo.GetAllAsync().Where(x => x.UserID == currentUser.Id).AsNoTracking().ToListAsync();
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

            var currentUser = await userManager.GetUserAsync(User);
            categoriesViewModel.UserID = currentUser.Id;

            await categoryRepo.CreateAsync(mapper.Map<Categories>(categoriesViewModel));
            toastNotification.AddSuccessToastMessage("Yeni Kategori Kaydı Başarılı!");

            return RedirectToAction(nameof(CategoryList));
        }

        [ServiceFilter(typeof(NotFoundFilter<Categories>))]
        public async Task<IActionResult> UpdateCategory(int id)
        {
            var data = await categoryRepo.GetByIdAsync(id);
            var mapData = mapper.Map<CategoriesViewModel>(data);

            return View(mapData);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCategory(CategoriesViewModel categoriesViewModel)
        {
            if (!ModelState.IsValid)
            {
                toastNotification.AddWarningToastMessage("Bilgiler Doğrulanamadı!");
                return RedirectToAction(nameof(UpdateCategory), new { id = categoriesViewModel.Id });
            }

            var currentUser = await userManager.GetUserAsync(User);
            categoriesViewModel.UserID = currentUser.Id;

            categoryRepo.Update(mapper.Map<Categories>(categoriesViewModel));
            toastNotification.AddSuccessToastMessage("Kategori Güncellendi");

            return RedirectToAction(nameof(CategoryList));
        }

        [ServiceFilter(typeof(NotFoundFilter<Categories>))]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var data = await categoryRepo.GetByIdAsync(id);
            categoryRepo.Delete(data);

            toastNotification.AddSuccessToastMessage("Kategori Silindi!");

            return RedirectToAction(nameof(CategoryList));
        }
    }
}
