using Microsoft.EntityFrameworkCore;

using Moq;
using TestTask.Data;
using TestTask.Models;
using TestTask.Models.Dto;
using TestTask.Services;

namespace TestTask.Tests.Services
{

    [TestFixture]
    public class CustomerServiceTests
    {
        private CustomerService customerService;
        private Mock<DataContext> mockContext;

        [SetUp]
        public void Setup()
        {
            mockContext = new Mock<DataContext>();
            customerService = new CustomerService(mockContext.Object);
        }

        [Test]
        public void AddCustomer_ValidCustomer_ReturnsNewCustomer()
        {
            // Arrange
            var customerDto = new CustomerDto
            {
                Name = "Test Customer",
                Email = "test@example.com"
            };

            mockContext.Setup(c => c.Add(It.IsAny<Customer>()));
            mockContext.Setup(c => c.SaveChanges());

            // Act
            var result = customerService.AddCustomer(customerDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Name, Is.EqualTo(customerDto.Name));
            Assert.That(result.Email, Is.EqualTo(customerDto.Email));
            Assert.That(result.OrdersCount, Is.EqualTo(0));
            Assert.IsEmpty(result.Orders);
        }

        [Test]
        public void GetCustomerById_ValidCustomerId_ReturnsCustomer()
        {
            // Arrange
            var customerId = 1;
            var customer = new Customer
            {
                Id = customerId,
                Name = "Test Customer",
                Email = "test@example.com"
            };
            var customers = new List<Customer>()
            {
                new Customer(){
                Id = customerId,
                Name = "Test Customer",
                Email = "test@example.com",
                Orders = new List<Order>()
            }}.AsQueryable();

            var data = new List<Order>()
            {
                new Order()
                {
                    Id = 1,
                    ItemName = "Item",
                    ItemDescription = "Description"
                }
            }.AsQueryable();

            var mockSet2 = new Mock<DbSet<Order>>();
            mockSet2.As<IQueryable<Order>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet2.As<IQueryable<Order>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet2.As<IQueryable<Order>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet2.As<IQueryable<Order>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());

            var mockSet = new Mock<DbSet<Customer>>();
            mockSet.As<IQueryable<Customer>>().Setup(m => m.Provider).Returns(customers.Provider);
            mockSet.As<IQueryable<Customer>>().Setup(m => m.Expression).Returns(customers.Expression);
            mockSet.As<IQueryable<Customer>>().Setup(m => m.ElementType).Returns(customers.ElementType);
            mockSet.As<IQueryable<Customer>>().Setup(m => m.GetEnumerator()).Returns(() => customers.GetEnumerator());

            mockContext.Setup(c => c.Customers).Returns(mockSet.Object);

            mockContext.Setup(c => c.Orders)
                .Returns(mockSet2.Object);

            // Act
            var result = customerService.GetCustomerById(customerId);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Id, Is.EqualTo(customerId));
            Assert.That(result.Name, Is.EqualTo(customer.Name));
            Assert.That(result.Email, Is.EqualTo(customer.Email));
        }

    }
}
