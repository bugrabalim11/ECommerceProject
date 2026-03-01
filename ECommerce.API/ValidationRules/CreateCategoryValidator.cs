using FluentValidation;
using ECommerce.API.DTOs.CategoryDtos;

namespace ECommerce.API.ValidationRules
{
    public class CreateCategoryValidator : AbstractValidator<CreateCategoryDto>
    {
        public CreateCategoryValidator()
        {
            RuleFor(x => x.CategoryName)
                .NotEmpty().WithMessage("Lütfen kategori adını boş geçmeyiniz!")
                .MinimumLength(3).WithMessage("Kategori adı en az 3 karakter olmalıdır!");

            RuleFor(x => x.CategoryDescription)
                .NotEmpty().WithMessage("Lütfen kategori açıklamasını boş geçmeyiniz!")
                .MinimumLength(10).WithMessage("Kategori açıklaması en az 10 karakter olabilir!")
                .MaximumLength(50).WithMessage("Kategori açıklaması en fazla 50 karakter olabilir!");
        }
    }
}
