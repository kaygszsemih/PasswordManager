using AutoMapper;
using PasswordManager.Models;
using PasswordManager.ViewModels;

namespace PasswordManager.Mapping
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<MyPasswords, MyPasswordsViewModel>().ReverseMap();
            CreateMap<MyPasswords, MyPasswordWithCategory>().ReverseMap();
            CreateMap<Categories, CategoriesViewModel>().ReverseMap();
            CreateMap<AppUser, UserInfoViewModel>().ReverseMap();
        }
    }
}
