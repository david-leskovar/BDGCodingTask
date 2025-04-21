using System.Globalization;
using BDGCodingTask.Services;
using BDGCodingTask.Services.Interfaces;

namespace BDGCodingTaskConsole
{
    public class Program
    {
        public static void Main(string[] args)
        {

            Console.WriteLine("Enter Order Type (Buy/Sell): ");
            string? ordertype = Console.ReadLine();
            if (ordertype != "Buy" && ordertype != "Sell")
            {
                Console.WriteLine("Invalid Order Type");
                return;
            }
            Console.WriteLine("Enter Order Amount: ");
            string? orderamountstr = Console.ReadLine();
            decimal orderamount = Decimal.TryParse(orderamountstr, out decimal result) ? result : 0;
            if (orderamount <= 0)
            {
                Console.WriteLine("Invalid amount");
                return;
            }


            IUserInstructionsService _userInstructionsService = new UserInstructionsService();
            IExchangeDataLoaderService _dataLoaderService = new ExchangeDataLoaderService();

            var userInstructions = _userInstructionsService.GetUserInstructions(ordertype, orderamount, _dataLoaderService.Exchanges);

            Console.WriteLine();
            Console.WriteLine($"Order {userInstructions.Item2.ToString()} Fullfilled.");
            Console.WriteLine($"Amount Fullfilled: {userInstructions.Item1.Sum(x => x.Amount)}");
            Console.WriteLine("User Instructions:");

            foreach (var userInstruction in userInstructions.Item1)
            {
                Console.WriteLine($"{userInstruction.OrderType} @Exchange: {userInstruction.ExchangeId}, Amount: {userInstruction.Amount}, Price: {userInstruction.Price}");
            }


        }

    }

}