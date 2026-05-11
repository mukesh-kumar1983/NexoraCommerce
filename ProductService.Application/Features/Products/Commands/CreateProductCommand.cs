using MediatR;

namespace ProductService.Application.Features.Products.Commands
{
    public record CreateProductCommand(
    string Name,
    decimal Price,
    int Stock
) : IRequest<Guid>;
}
