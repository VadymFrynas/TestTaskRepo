using Microsoft.EntityFrameworkCore;

using TestTask.Data;
using TestTask.Models;
using TestTask.Models.Dto;
using TestTask.Services.Interfaces;

namespace TestTask.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly DataContext context;

        public CustomerService(DataContext context)
        {
            this.context = context;
        }
        public Customer AddCustomer(CustomerDto customerDto)
        {
            var customer = new Customer()
            {
                Name = customerDto.Name,
                Email = customerDto.Email,
                OrdersCount = 0,
                Orders = new List<Order>()
            };

            context.Add(customer);
            context.SaveChanges();

            return customer;
        }

        public Customer GetCustomerById(int customerId)
        {
            return context.Customers.Include(c => c.Orders).FirstOrDefault(c => c.Id == customerId);
        }

        public List<Customer> GetCustomers()
        {
            return context.Customers.Include(c => c.Orders).ToList();
        }
    }
}
