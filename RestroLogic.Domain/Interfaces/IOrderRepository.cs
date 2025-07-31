using RestroLogic.Domain.Entities;

namespace RestroLogic.Domain.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order> GetByIdAsync(Guid id);
        Task AddAsync(Order order);
        Task UpdateAsync(Order order);
        Task DeleteAsync(Order order);
    }
}
