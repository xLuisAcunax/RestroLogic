using RestroLogic.Domain.Sales;
using RestroLogic.Domain.ValueObjects;
using Xunit;

namespace RestroLogic.Domain.Tests.Sales
{
    public class OrderTests
    {
        [Fact]
        public void CannotCloseEmptyOrder()
        {
            var order = Order.Open(1);
            Assert.Throws<InvalidOperationException>(() => order.Close());
        }


        [Fact]
        public void TotalSumsItems()
        {
            var order = Order.Open(1);
            order.AddItem(Guid.NewGuid(), "Café", Money.From(5000), Quantity.From(2));
            order.AddItem(Guid.NewGuid(), "Pan", Money.From(3000), Quantity.From(1));
            Assert.Equal(13000m, order.Total().Amount);
        }
    }
}
