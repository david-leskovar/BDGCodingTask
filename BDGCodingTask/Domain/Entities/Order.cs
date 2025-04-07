using BDGCodingTask.Domain.Enums;

namespace BDGCodingTask.Domain.Entities
{
    public class Order
    {
        public String? Id { get; set; }
        public DateTime Time { get; set; }
        public String? Kind { get; set; }
        public OrderType Type { get; set; }
        public decimal Amount { get; set; }
        public decimal Price { get; set; }

    }
}
