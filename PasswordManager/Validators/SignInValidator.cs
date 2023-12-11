using FluentValidation;
using PasswordManager.ViewModels;

namespace PasswordManager.Validators
{
    public class SignInValidator : AbstractValidator<SignInViewModel>
    {
        public SignInValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("E-Posta Adresi Boş Bırakılamaz.").NotNull().WithMessage("E-Posta Adresi Boş Bırakılamaz.");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Şifre Alanı Boş Bırakılamaz.").NotNull().WithMessage("Şifre Alanı Boş Bırakılamaz.");
        }
    }
}
