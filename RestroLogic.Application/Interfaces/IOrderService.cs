using RestroLogic.Application.Common.Pagination;
using RestroLogic.Application.Dtos.Orders;

namespace RestroLogic.Application.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDto>> GetAllAsync();
        Task<OrderDto?> GetByIdAsync(Guid id);
        Task<Guid> CreateAsync(CreateOrderDto dto);
        Task<bool> UpdateAsync(Guid id, UpdateOrderDto dto);
        Task<bool> DeleteAsync(Guid id);
        Task<PagedResult<OrderDto>> SearchAsync(OrderQueryParams qp, CancellationToken ct = default);
    }
}
