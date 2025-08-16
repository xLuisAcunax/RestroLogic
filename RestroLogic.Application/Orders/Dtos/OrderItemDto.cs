namespace RestroLogic.Application.Orders.Dtos
{
    public record OrderItemDto(
        Guid ProductId, 
        string Name, 
        int Qty, 
        decimal UnitPrice, 
        decimal LineTotal
    );
}
