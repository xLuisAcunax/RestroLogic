using RestroLogic.Application.Dtos.Orders;
using RestroLogic.Application.Interfaces;
using RestroLogic.Domain.Entities;
using RestroLogic.Domain.Interfaces;

namespace RestroLogic.Application.Services.Orders
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _repository;

        public OrderService(IOrderRepository repository) 
        {
            _repository = repository;
        }

        public async Task<IEnumerable<OrderDto>> GetAllAsync()
        {
            var orders = await _repository.GetAllAsync();
            return orders.Select(MapToDto);
        }

        public async Task<OrderDto?> GetByIdAsync(Guid id)
        {
            var order = await _repository.GetByIdAsync(id);
            return order is null ?  null : MapToDto(order);
        }

        public async Task<Guid> CreateAsync(CreateOrderDto dto)
        {
            if (dto.Items == null || !dto.Items.Any())
                throw new ArgumentException("Una orden debe contener almenos un ítem.");

            var order = new Order(dto.CustomerId);
            foreach (var item in dto.Items)
            {
                var product = new Product(item.ProductName, item.ProductDescription, item.UnitPrice);
                order.AddItem(product, item.Quantity);
            }

            await _repository.AddAsync(order);
            return order.Id;
        }

        public async Task<bool> UpdateAsync(Guid id, UpdateOrderDto dto)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing is null)
                return false;

            var updated = new Order(dto.CustomerId);
            foreach (var item in dto.Items)
            {
                var product = new Product(item.ProductName, item.ProductDescription, item.UnitPrice);
                updated.AddItem(product, item.Quantity);
            }

            await _repository.UpdateAsync(updated);
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing is null)
                return false;

            await _repository.DeleteAsync(existing);
            return true;
        }

        private static OrderDto MapToDto(Order order)
        {
            return new OrderDto
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                OrderDate = order.CreatedAt,
                Total = order.GetTotal(),
                Items = order.Items.Select(i => new OrderItemDto
                {
                    ProductId = i.ProductId,
                    ProductName = i.ProductName,
                    UnitPrice = i.UnitPrice,
                    Quantity = i.Quantity
                }).ToList()
            };
        }
    }
}
