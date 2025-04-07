using System.Text.Json.Serialization;
using BDGCodingTask.Data;

namespace BDGCodingTask.Domain.Entities
{
    public class Exchange
    {
        public Guid? Id { get; set; }
        public String? Name { get; set; }
        public DateTime AcqTime { get; set; }

        private decimal _btcBalance;
        public decimal BTCBalance
        {
            get { return _btcBalance; }
            set
            {

                if (_btcBalance + value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "BTC balance cannot be negative.");
                }
                _btcBalance = value;
            }
        }

        private decimal _eurBalance;
        public decimal EURBalance
        {
            get { return _eurBalance; }
            set
            {
                if (_eurBalance + value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "EUR balance cannot be negative.");
                }
                _eurBalance = value;
            }
        }

        [JsonConverter(typeof(OrderConverter))]
        public List<Order> Bids { get; set; } = new List<Order>();

        [JsonConverter(typeof(OrderConverter))]
        public List<Order> Asks { get; set; } = new List<Order>();

    }
}
