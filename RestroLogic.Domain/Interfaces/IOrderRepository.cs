using RestroLogic.Domain.Entities;

namespace RestroLogic.Domain.Interfaces
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllAsync();
        Task<IEnumerable<Order>> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default);
        Task<Order> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task AddAsync(Order order, CancellationToken cancellationToken = default);
        Task UpdateAsync(Order order, CancellationToken cancellationToken = default);
        Task DeleteAsync(Order order, CancellationToken cancellationToken = default);
        Task<(IEnumerable<Order> Items, int Total)> SearchAsync(
        Guid? customerId,
        DateTime? from,
        DateTime? to,
        string? sortBy,
        bool desc,
        int page,
        int pageSize,
        CancellationToken ct = default);
    }
}
