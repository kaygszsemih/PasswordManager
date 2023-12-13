using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using PasswordManager.Models;
using PasswordManager.Repositories;
using PasswordManager.ViewModels;
using System.Diagnostics;

namespace PasswordManager.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly MyPasswordsRepo passwordRepo;
        private readonly IMapper mapper;

        public HomeController(UserManager<AppUser> userManager, MyPasswordsRepo passwordRepo, IMapper mapper)
        {
            this.userManager = userManager;
            this.passwordRepo = passwordRepo;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var currentUser = await userManager.GetUserAsync(User);

            var data = await passwordRepo.GetAllAsync().Where(x => x.UserID == currentUser.Id).Include(x => x.Categories).AsNoTracking().ToListAsync();
            var mapData = mapper.Map<List<MyPasswordWithCategory>>(data);

            return View(mapData);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(ErrorViewModel errorViewModel)
        {
            return View(errorViewModel);
        }
    }
}
