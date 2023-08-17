using System.ComponentModel.DataAnnotations.Schema;
namespace TestTask.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public string ItemName { get; set; }
        public string ItemDescription { get; set; }
        public OrderStatus Status { get; set; }
        public float Quantity { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public float CurrentPrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public float TotalPrice { get; set; }
    }
    public enum OrderStatus
    {
        Pending,
        Shipped,
        Delivered,
        Cancelled
    }
}
