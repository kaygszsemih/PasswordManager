using FluentValidation;
using PasswordManager.ViewModels;

namespace PasswordManager.Validators
{
    public class CategoriesValidator : AbstractValidator<CategoriesViewModel>
    {
        public CategoriesValidator()
        {
            RuleFor(x => x.CategoryName).NotEmpty().WithMessage("Kategori Adı Boş Olamaz.").NotNull().WithMessage("Kategori Adı Boş Olamaz.");
        }
    }
}
