using FluentValidation;
using PasswordManager.ViewModels;

namespace PasswordManager.Validators
{
    public class SignUpValidator : AbstractValidator<SignUpViewModel>
    {
        public SignUpValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Kullanıcı Adı Boş Bırakılamaz.").NotNull().WithMessage("Kullanıcı Adı Boş Bırakılamaz.");
            RuleFor(x => x.Email).NotEmpty().WithMessage("E-Posta Adresi Boş Bırakılamaz.").NotNull().WithMessage("E-Posta Adresi Boş Bırakılamaz.");
            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Telefon No Boş Bırakılamaz.").NotNull().WithMessage("Telefon No Boş Bırakılamaz.").MaximumLength(10).WithMessage("Telefon Numarasını 10 Hane Olarak Giriniz.");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Şifre Alanı Boş Bırakılamaz.").NotNull().WithMessage("Şifre Alanı Boş Bırakılamaz.").MinimumLength(6).WithMessage("Şifre En Az 6 Karakter Olabilir.");
            RuleFor(x => x.PasswordConfirm).NotEmpty().WithMessage("Şifre Alanı Boş Bırakılamaz.").NotNull().WithMessage("Şifre Alanı Boş Bırakılamaz.").MinimumLength(6).WithMessage("Şifre En Az 6 Karakter Olabilir.").Equal(x => x.Password).WithMessage("Şifreler Eşleşmiyor.");
        }
    }
}
