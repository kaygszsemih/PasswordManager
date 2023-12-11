using FluentValidation;
using PasswordManager.ViewModels;

namespace PasswordManager.Validators
{
    public class ForgetPasswordValidator : AbstractValidator<ForgetPasswordViewModel>
    {
        public ForgetPasswordValidator()
        {
            RuleFor(x => x.Email).NotNull().WithMessage("E-Posta Adresi Boş Olamaz.").NotEmpty().WithMessage("E-Posta Adresi Boş Olamaz.");
        }
    }
}
