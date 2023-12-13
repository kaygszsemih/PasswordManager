using FluentValidation;
using PasswordManager.ViewModels;

namespace PasswordManager.Validators
{
    public class UserInfoValidator  : AbstractValidator<UserInfoViewModel>
    {
        public UserInfoValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Kullanıcı Adı Boş Bırakılamaz.").NotNull().WithMessage("Kullanıcı Adı Boş Bırakılamaz.");
            RuleFor(x => x.Email).NotEmpty().WithMessage("E-Posta Adresi Boş Bırakılamaz.").NotNull().WithMessage("E-Posta Adresi Boş Bırakılamaz.");
            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Telefon No Boş Bırakılamaz.").NotNull().WithMessage("Telefon No Boş Bırakılamaz.").MaximumLength(10).WithMessage("Telefon Numarasını 10 Hane Olarak Giriniz.");
        }
    }
}
