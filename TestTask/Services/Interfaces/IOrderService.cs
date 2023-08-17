using Microsoft.AspNetCore.JsonPatch;

using TestTask.Models;
using TestTask.Models.Dto;

namespace TestTask.Services.Interfaces
{
    public interface IOrderService
    {
        Task<Order> PlaceOrder(OrderDto orderDto);
        List<Order> GetOrders();
        Order GetOrderDetails(int orderId);
    }
}
