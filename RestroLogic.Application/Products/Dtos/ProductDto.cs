namespace RestroLogic.Application.Products.Dtos
{
    public record ProductDto(
        Guid Id, 
        string Name, 
        string? Description, 
        decimal Price, 
        bool IsActive
    );
}
