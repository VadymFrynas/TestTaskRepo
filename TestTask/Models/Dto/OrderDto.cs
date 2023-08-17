using System.ComponentModel.DataAnnotations.Schema;

namespace TestTask.Models.Dto
{
    public class OrderDto
    {
        public int UserId { get; set; }
        public string ItemName { get; set; }
        public string ItemDescription { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public float Quantity { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public float CurrentPrice { get; set; }
    }
}
