using RestroLogic.Domain.Entities;
using Xunit;

namespace RestroLogic.Domain.Tests;

public class OrderTests
{
    [Fact]
    public void AddItem_MergesQuantity()
    {
        var order = new Order(Guid.NewGuid());
        var product = new Product("Burger", "Delicious", 10m);

        order.AddItem(product, 2);
        order.AddItem(product, 3);

        var item = Assert.Single(order.Items);
        Assert.Equal(5, item.Quantity);
    }
}

