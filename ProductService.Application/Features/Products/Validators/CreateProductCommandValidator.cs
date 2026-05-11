using FluentValidation;
using ProductService.Application.Features.Products.Commands;

namespace ProductService.Application.Features.Products.Validators
{
    public class CreateProductCommandValidator
    : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Price)
                .GreaterThan(0);
        }
    }
}
