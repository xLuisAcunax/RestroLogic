using MediatR;
using RestroLogic.Application.Common.Results;
using RestroLogic.Application.Dtos.Sales;

namespace RestroLogic.Application.Commands.Sales.AddItem
{
    public sealed record AddItemCommand(
    Guid OrderId,
    Guid MenuItemId,
    string MenuItemName,
    decimal UnitPrice,
    string Currency,
    int Quantity
    ) : IRequest<Result<OrderDto>>;
}
