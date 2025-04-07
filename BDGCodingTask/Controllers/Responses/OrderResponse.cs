using BDGCodingTask.Domain.Entities;

namespace BDGCodingTask.Controllers.Responses
{
    public class OrderResponse
    {
        public string message { get; set; } = string.Empty;
        public decimal amountFullfilled { get; set; }
        public List<UserInstruction> userInstructions { get; set; } = new List<UserInstruction>();
    }
}
