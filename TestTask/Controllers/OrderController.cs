using Azure.Messaging.ServiceBus;

using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using TestTask.Data;
using TestTask.Models;
using TestTask.Models.Dto;
using TestTask.Services.Interfaces;

namespace TestTask.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly DataContext dbContext;
        private readonly IOrderService orderService;

        public OrderController(DataContext dbContext, IOrderService orderService)
        {
            this.dbContext = dbContext;
            this.orderService = orderService;
        }

        [HttpGet]
        public IActionResult GetOrders()
        {
            var orders = orderService.GetOrders();
            return Ok(orders);
        }

        [HttpPost]
        public async Task<Order> CreateOrder([FromBody] OrderDto orderDto)
        {

            return await orderService.PlaceOrder(orderDto);
        }

        [HttpGet("{orderId}")]
        public IActionResult GetOrder(int orderId)
        {
            if (orderId == 0)
            {
                return BadRequest("Order Id can not be 0");
            }

            var order = orderService.GetOrderDetails(orderId);

            if (order == null)
                return NotFound($"Cannot find order with this Id : {orderId}");

            return Ok(order);
        }

        [HttpPatch("{orderId}")]
        public IActionResult UpdateOrder(int orderId, [FromBody] JsonPatchDocument<Order> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            var order = dbContext.Orders.FirstOrDefault(o => o.Id == orderId);

            if (order == null)
            {
                return NotFound();
            }

            patchDoc.ApplyTo(order, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            dbContext.SaveChanges();

            return Ok(order);
        }
    }
}
