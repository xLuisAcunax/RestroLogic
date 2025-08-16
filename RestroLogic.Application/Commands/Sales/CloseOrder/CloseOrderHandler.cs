using MediatR;
using Microsoft.EntityFrameworkCore;
using RestroLogic.Application.Common.Results;
using RestroLogic.Infrastructure.Persistence;

namespace RestroLogic.Application.Commands.Sales.CloseOrder
{
    public sealed class CloseOrderHandler : IRequestHandler<CloseOrderCommand, Result<decimal>>
    {
        private readonly AppDbContext _db;
        public CloseOrderHandler(AppDbContext db) => _db = db;


        public async Task<Result<decimal>> Handle(CloseOrderCommand req, CancellationToken ct)
        {
            var order = await _db.Orders.Include(o => o.Items).FirstOrDefaultAsync(o => o.Id == req.OrderId, ct);
            if (order is null)
                return Result<decimal>.Failure("Order not found");


            try
            {
                order.Close();
            }
            catch (InvalidOperationException ex)
            {
                return Result<decimal>.Failure(ex.Message);
            }


            await _db.SaveChangesAsync(ct);
            return Result<decimal>.Success(order.Total().Amount);
        }
    }
}
