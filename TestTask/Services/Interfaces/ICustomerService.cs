using TestTask.Models;
using TestTask.Models.Dto;

namespace TestTask.Services.Interfaces
{
    public interface ICustomerService
    {
        Customer AddCustomer(CustomerDto customerDto);
        List<Customer> GetCustomers();
        Customer GetCustomerById(int customerId);
    }
}
