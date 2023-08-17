using Microsoft.AspNetCore.Mvc;

using TestTask.Models.Dto;
using TestTask.Services.Interfaces;

namespace TestTask.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService customerService;

        public CustomerController(ICustomerService customerService)
        {
            this.customerService = customerService;
        }

        [HttpGet]
        public IActionResult GetCustomers()
        {
            var customers = customerService.GetCustomers();
            return Ok(customers);
        }

        [HttpPost]
        public IActionResult CreateCustomer(CustomerDto customerDto)
        {
            var customer = customerService.AddCustomer(customerDto);
            return Ok(customer);
        }

        [HttpGet("{customerId}")]
        public IActionResult GetCustomer(int customerId)
        {
            if (customerId == 0)
            {
                return BadRequest("Customer Id can not be 0");
            }

            var order = customerService.GetCustomerById(customerId);

            if (order == null)
                return NotFound($"Cannot find customer with this Id : {customerId}");

            return Ok(order);
        }
    }
}
