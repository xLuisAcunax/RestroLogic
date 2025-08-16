namespace RestroLogic.WebApi.Contracts
{
    public sealed record AddItemRequest(
        Guid MenuItemId,
        string MenuItemName,
        decimal UnitPrice,
        string? Currency,
        int Quantity
    );
}
