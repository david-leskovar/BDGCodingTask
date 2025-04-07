using System.ComponentModel.DataAnnotations;

namespace BDGCodingTask.Controllers.Requests
{
    public class OrderRequest
    {
        [Required]
        [RegularExpression("^(Buy|Sell)$", ErrorMessage = "OrderType must be either 'Buy' or 'Sell'.")]
        public string OrderType { get; set; }

        [Required]
        [Range(0.00000001, double.MaxValue, ErrorMessage = "Amount must be a positive number.")]
        public decimal Amount { get; set; }
    }
}
