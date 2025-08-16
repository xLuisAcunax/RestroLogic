using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestroLogic.Application.Orders.Dtos
{
    public record OrderSummaryDto(
        Guid OrderId, 
        Guid TableId, 
        DateTime OpenedAt, 
        string Status, 
        decimal Total, 
        IReadOnlyList<OrderItemDto> Items
    );
}
