using MediatR;
using ProductService.Application.Features.Products.Commands;

namespace ProductService.Application.Features.Products.Handlers
{
    public class CreateProductCommandHandler
    : IRequestHandler<CreateProductCommand, Guid>
    {
        private readonly IApplicationDbContext _context;

        public CreateProductCommandHandler(
            IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Handle(
            CreateProductCommand request,
            CancellationToken cancellationToken)
        {
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Price = request.Price,
                Stock = request.Stock
            };

            _context.Products.Add(product);

            await _context.SaveChangesAsync(cancellationToken);

            return product.Id;
        }
    }
}
