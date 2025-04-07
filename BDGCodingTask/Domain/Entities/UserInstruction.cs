using BDGCodingTask.Domain.Enums;

namespace BDGCodingTask.Domain.Entities
{
    public class UserInstruction
    {
        public Guid Id { get; set; }
        public string? ExchangeId { get; set; }
        public decimal Price { get; set; }
        public decimal Amount { get; set; }
        public OrderType OrderType { get; set; }
    }
}
