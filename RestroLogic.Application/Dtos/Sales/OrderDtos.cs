namespace RestroLogic.Application.Dtos.Sales
{
    public sealed record OrderSummaryDto(
    Guid Id,
    int TableNumber,
    bool IsClosed,
    DateTime CreatedAt,
    DateTime? ClosedAt,
    int ItemsCount,
    decimal Total,
    string Currency
    );


    public sealed record OrderDto(
    Guid Id,
    int TableNumber,
    bool IsClosed,
    DateTime CreatedAt,
    DateTime? ClosedAt,
    decimal Total,
    string Currency,
    IReadOnlyList<OrderItemDto> Items
    );


    public sealed record OrderItemDto(
    Guid Id,
    Guid MenuItemId,
    string MenuItemName,
    int Quantity,
    decimal UnitPrice,
    string Currency,
    decimal Subtotal
    );
}
