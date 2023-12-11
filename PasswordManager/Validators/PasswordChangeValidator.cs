using FluentValidation;
using PasswordManager.ViewModels;

namespace PasswordManager.Validators
{
    public class PasswordChangeValidator : AbstractValidator<PasswordChangeViewModel>
    {
        public PasswordChangeValidator()
        {
            RuleFor(x => x.OldPassword).NotEmpty().WithMessage("Şifre Alanı Boş Bırakılamaz.").NotNull().WithMessage("Şifre Alanı Boş Bırakılamaz.").MinimumLength(6).WithMessage("Şifre En Az 6 Karakter Olabilir.");
            RuleFor(x => x.NewPassword).NotEmpty().WithMessage("Yeni Şifre Alanı Boş Bırakılamaz.").NotNull().WithMessage("Yeni Şifre Alanı Boş Bırakılamaz.").MinimumLength(6).WithMessage("Şifre En Az 6 Karakter Olabilir.");
            RuleFor(x => x.ConfirmPassword).NotEmpty().WithMessage("Yeni Şifre Tekrar Alanı Boş Bırakılamaz.").NotNull().WithMessage("Yeni Şifre Tekrar Alanı Boş Bırakılamaz.").MinimumLength(6).WithMessage("Şifre En Az 6 Karakter Olabilir.").Equal(x => x.NewPassword).WithMessage("Şifreler Eşlemiyor.");
        }
    }
}
