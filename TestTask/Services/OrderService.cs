
using TestTask.Data;
using TestTask.Models;
using TestTask.Models.Dto;
using TestTask.Services.Interfaces;

namespace TestTask.Services
{
    public class OrderService : IOrderService
    {
        private readonly DataContext context;
        private readonly IServiceBus serviceBus;

        public OrderService(DataContext context, IServiceBus serviceBus)
        {
            this.context = context;
            this.serviceBus = serviceBus;
        }
        public async Task<Order> PlaceOrder(OrderDto orderDto)
        {
            var order = new Order
            {
                UserId = orderDto.UserId,
                OrderDate = DateTime.Now,
                ItemName = orderDto.ItemName,
                ItemDescription = orderDto.ItemDescription,
                Quantity = orderDto.Quantity,
                CurrentPrice = orderDto.CurrentPrice,
                TotalPrice = orderDto.Quantity * orderDto.CurrentPrice,
                Status = orderDto.OrderStatus
            };

            var customer = context.Customers.FirstOrDefault(c => c.Id == order.UserId);

            if (customer == null) 
            {
                throw new Exception($"Customer with this id: {order.UserId} doesnt exist");
            }
            if (customer.Orders == null) 
            {
                customer.Orders = new List<Order>();
            }
            customer.Orders.Add(order);
            
            context.SaveChanges();

            await serviceBus.SendMessageAsync(order.Id);

            return order;
        }

        public List<Order> GetOrders()
        {
            return context.Orders.ToList();
        }

        public Order GetOrderDetails(int orderId)
        {
            var order = context.Orders
                .FirstOrDefault(o => o.Id == orderId);

            return order;
        }

    }
}
