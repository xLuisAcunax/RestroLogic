using MediatR;
using RestroLogic.Application.Products.Commands;
using RestroLogic.Domain.Entities;
using RestroLogic.Infrastructure.Persistence;

namespace RestroLogic.Application.Products.Handlers
{
    public sealed class CreateProductHandler : IRequestHandler<CreateProductCommand, Guid>
    {
        private readonly AppDbContext _db;
        public CreateProductHandler(AppDbContext db) => _db = db;

        public async Task<Guid> Handle(CreateProductCommand request, CancellationToken ct)
        {
            var entity = new Product(request.Name, request.Description, request.Price);
            _db.Products.Add(entity);
            await _db.SaveChangesAsync(ct);
            return entity.Id;
        }
    }
}
