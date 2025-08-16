using MediatR;
using RestroLogic.Application.Common.Results;
using RestroLogic.Domain.Sales;
using RestroLogic.Infrastructure.Persistence;

namespace RestroLogic.Application.Commands.Sales.CreateOrder
{
    public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, Result<Guid>>
    {
        private readonly AppDbContext _db;


        public CreateOrderHandler(AppDbContext db) => _db = db;


        public async Task<Result<Guid>> Handle(CreateOrderCommand request, CancellationToken ct)
        {
            var order = Order.Open(request.TableNumber);
            _db.Orders.Add(order);
            await _db.SaveChangesAsync(ct);
            return Result<Guid>.Success(order.Id);
        }
    }
}
