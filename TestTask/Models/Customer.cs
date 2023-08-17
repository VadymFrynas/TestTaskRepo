namespace TestTask.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int OrdersCount { get; set; }

        public List<Order> Orders { get; set; }
    }
}
