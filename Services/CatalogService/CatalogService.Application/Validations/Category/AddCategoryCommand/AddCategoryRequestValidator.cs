using CatalogService.Application.CategoryHandlers.AddCategoryCommand;
using FluentValidation;

namespace CatalogService.Application.Validations.Category.AddCategoryCommand
{
    public class AddCategoryRequestValidator : AbstractValidator<AddCategoryRequest>
    {
        public AddCategoryRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Kategori adı boş geçilemez.")
                .MinimumLength(3).WithMessage("Kategori adı en az 3 karakter olmalıdır.")
                .MaximumLength(100).WithMessage("Kategori adı en falza 100 karakter olmalıdır.");
        }
    }
}