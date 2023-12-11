using FluentValidation;
using PasswordManager.ViewModels;

namespace PasswordManager.Validators
{
    public class MyPasswordsValidator : AbstractValidator<MyPasswordsViewModel>
    {
        public MyPasswordsValidator()
        {
            RuleFor(x => x.Description).NotEmpty().WithMessage("Şifre Tanımı Boş Olamaz.").NotNull().WithMessage("Şifre Tanımı Boş Olamaz.");
            RuleFor(x => x.URL).NotEmpty().WithMessage("Platform Adresi Boş Olamaz.").NotNull().WithMessage("Platform Adresi Boş Olamaz.");
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Kullanıcı Adı Boş Olamaz.").NotNull().WithMessage("Kullanıcı Adı Boş Olamaz.");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Şifre Boş Olamaz.").NotNull().WithMessage("Şifre Boş Olamaz.");
            RuleFor(x => x.CategoryID).NotEmpty().WithMessage("Kategori Boş Olamaz.").NotNull().WithMessage("Kategori Boş Olamaz.");
        }
    }
}
