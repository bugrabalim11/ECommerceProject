using FluentValidation;
using ECommerce.API.DTOs.ProductDtos;

namespace ECommerce.API.ValidationRules
{
    // AbstractValidator'dan miras alarak sisteme "Ben bir güvenlik görevlisiyim" diyoruz
    public class CreateProductValidator : AbstractValidator<CreateProductDto>
    {
        // Kuralları bu Constructor (yapıcı metot) içine yazarız
        public CreateProductValidator()
        {
            // Kural 1: Ürün adı boş olamaz
            RuleFor(x => x.ProductName)
                .NotEmpty().WithMessage("Lütfen ürün adını boş geçmeyiniz!")
                .MinimumLength(3).WithMessage("Ürün adı en az 3 karakter olmalıdır!");

            // Kural 2: Fiyat 0 veya negatif olamaz
            RuleFor(x => x.ProductPrice)
                .GreaterThan(0).WithMessage("Ürün fiyatı 0'dan büyük olmak zorundadır!");

            // Kural 3: Stok adedi negatif olamaz
            RuleFor(x => x.StockQuantity)
                .GreaterThanOrEqualTo(0).WithMessage("Stok adedi negatif olamaz!");
        }
    }
}
