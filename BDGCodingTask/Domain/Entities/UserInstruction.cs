namespace BDGCodingTask.Domain.Entities
{
    class UserInstruction
    {
        public Guid Id { get; set; }
        public string? ExchangeId { get; set; }
        public decimal Price { get; set; }
        public decimal Amount { get; set; }
    }
}
