using Microsoft.EntityFrameworkCore;
using Moq;
using TestTask.Data;
using TestTask.Models;
using TestTask.Models.Dto;
using TestTask.Services;
using TestTask.Services.Interfaces;

namespace TestTask.Tests.Services
{
    [TestFixture]
    public class OrderServiceTests
    {
        private OrderService orderService;
        private Mock<DataContext> mockContext;
        private Mock<IServiceBus> mockServiceBus;

        [SetUp]
        public void Setup()
        {
            mockContext = new Mock<DataContext>();
            mockServiceBus = new Mock<IServiceBus>();
            orderService = new OrderService(mockContext.Object, mockServiceBus.Object);
        }

        [Test]
        public async Task PlaceOrder_ValidOrder_ReturnsNewOrder()
        {
            // Arrange
            var customers = new List<Customer>()
            {
                new Customer { Id = 1, Orders = new List<Order>() }
            }.AsQueryable();
            var orderDto = new OrderDto
            {
                UserId = 1,
                ItemName = "Test Item",
                ItemDescription = "Test Description",
                Quantity = 2,
                CurrentPrice = 10,
                OrderStatus = OrderStatus.Pending
            };

            var mockSet = new Mock<DbSet<Customer>>();
            mockSet.As<IQueryable<Customer>>().Setup(m => m.Provider).Returns(customers.Provider);
            mockSet.As<IQueryable<Customer>>().Setup(m => m.Expression).Returns(customers.Expression);
            mockSet.As<IQueryable<Customer>>().Setup(m => m.ElementType).Returns(customers.ElementType);
            mockSet.As<IQueryable<Customer>>().Setup(m => m.GetEnumerator()).Returns(() => customers.GetEnumerator());
            mockContext.Setup(c => c.Customers)
                .Returns(mockSet.Object);

            mockContext.Setup(c => c.Add(It.IsAny<Order>()));
            mockContext.Setup(c => c.SaveChanges());
            mockServiceBus.Setup(s => s.SendMessageAsync(It.IsAny<int>()));

            // Act
            var result = await orderService.PlaceOrder(orderDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.UserId, Is.EqualTo(orderDto.UserId));
            Assert.That(result.ItemName, Is.EqualTo(orderDto.ItemName));
            Assert.That(result.TotalPrice, Is.EqualTo(orderDto.Quantity * orderDto.CurrentPrice));
        }

        [Test]
        public void GetOrders_ReturnsListOfOrders()
        {
            // Arrange
            var orders = new List<Order>
            {
                new Order { Id = 1, ItemName = "Item 1" },
                new Order { Id = 2, ItemName = "Item 2" }
            }.AsQueryable();
            
            var mockSet = new Mock<DbSet<Order>>();
            mockSet.As<IQueryable<Order>>().Setup(m => m.Provider).Returns(orders.Provider);
            mockSet.As<IQueryable<Order>>().Setup(m => m.Expression).Returns(orders.Expression);
            mockSet.As<IQueryable<Order>>().Setup(m => m.ElementType).Returns(orders.ElementType);
            mockSet.As<IQueryable<Order>>().Setup(m => m.GetEnumerator()).Returns(() => orders.GetEnumerator());

            mockContext.Setup(c => c.Orders).Returns(mockSet.Object);
            
            // Act
            var result = orderService.GetOrders();

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0].ItemName, Is.EqualTo("Item 1"));
            Assert.That(result[1].ItemName, Is.EqualTo("Item 2"));
        }

    }
}
