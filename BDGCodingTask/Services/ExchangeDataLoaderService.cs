using BDGCodingTask.Domain.Entities;
using BDGCodingTask.Services.Interfaces;
using Newtonsoft.Json;

namespace BDGCodingTask.Services
{
    public class ExchangeDataLoaderService: IExchangeDataLoaderService
    {

        public List<Exchange> Exchanges { get; set; } = new List<Exchange>();

        public ExchangeDataLoaderService()
        {
            var exchanges = ParseExchangeData("Data/input.txt");
            if (exchanges != null)
            {
                Exchanges = exchanges;
            }
        }

        private static List<Exchange>? ParseExchangeData(string filePath)
        {

            List<Exchange> exchanges = new List<Exchange>();
            try
            {
                var lines = File.ReadLines(filePath);
                foreach (var line in lines)
                {
                    string jsonPart = line.Substring(line.IndexOf("{"));
                    var exchange = JsonConvert.DeserializeObject<Exchange>(jsonPart);

                    if (exchange == null)
                    {
                        throw new Exception("Exchange not parsed correctly");
                    }

                    exchange.Id = Guid.NewGuid();
                    exchange.BTCBalance = 1000;
                    exchange.EURBalance = 10000000;
                    exchanges.Add(exchange);

                }
                return exchanges;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while reading the file: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                return null;
            }
        }



    }
}
