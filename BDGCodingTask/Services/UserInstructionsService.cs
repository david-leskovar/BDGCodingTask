using BDGCodingTask.Domain.Entities;
using BDGCodingTask.Domain.Enums;
using BDGCodingTask.Services.Interfaces;

namespace BDGCodingTask.Services
{
    public class UserInstructionsService : IUserInstructionsService
    {
        public (List<UserInstruction>,UserInstructionCompletion) GetUserInstructions(string orderType, decimal amount, List<Exchange> exchanges)
        {
            decimal amountFullfilled = 0;
            List<UserInstruction> userOrders = new List<UserInstruction>();

            if (orderType == "Sell")
            {

                var bidsWithExchange = exchanges
               .SelectMany(exchange => exchange.Bids, (exchange, bid) => new { exchange, bid })
               .OrderByDescending(order => order.bid.Price)
               .ToList();

                foreach (var bidWithExchange in bidsWithExchange)
                {
                    if (amountFullfilled >= amount)
                    {
                        break;
                    }

                    var amounttosell = Math.Min(Math.Min(bidWithExchange.bid.Amount, amount - amountFullfilled), bidWithExchange.exchange.EURBalance / bidWithExchange.bid.Price);

                    if (amounttosell == 0)
                    {
                        continue;
                    }


                    bidWithExchange.exchange.EURBalance -= amounttosell * bidWithExchange.bid.Price;
                    bidWithExchange.exchange.BTCBalance += amounttosell;
                    amountFullfilled += amounttosell;
                    bidWithExchange.bid.Amount -= amounttosell;

                    userOrders.Add(new UserInstruction
                    {
                        Id = Guid.NewGuid(),
                        ExchangeId = bidWithExchange.exchange.Id.ToString(),
                        Price = bidWithExchange.bid.Price,
                        Amount = amounttosell,
                        OrderType = OrderType.Sell
                    });

                    if (bidWithExchange.bid.Amount <= 0)
                    {
                        exchanges.Where(e => e.Id == bidWithExchange.exchange.Id)
                        .FirstOrDefault()
                        ?.Bids
                        .Remove(bidWithExchange.bid);
                    }

                }

            }


            else if (orderType == "Buy")
            {

                var asksWithExchange = exchanges
               .SelectMany(exchange => exchange.Asks, (exchange, ask) => new { exchange, ask })
               .OrderBy(order => order.ask.Price)
               .ToList();

                foreach (var askWithExchange in asksWithExchange)
                {
                    if (amountFullfilled >= amount)
                    {
                        break;
                    }

                    var amounttobuy = Math.Min(Math.Min(askWithExchange.ask.Amount, amount - amountFullfilled), askWithExchange.exchange.BTCBalance);

                    if (amounttobuy == 0)
                    {
                        continue;
                    }


                    askWithExchange.exchange.EURBalance += amounttobuy * askWithExchange.ask.Price;
                    askWithExchange.exchange.BTCBalance -= amounttobuy;
                    amountFullfilled += amounttobuy;


                    userOrders.Add(new UserInstruction
                    {
                        Id = Guid.NewGuid(),
                        ExchangeId = askWithExchange.exchange.Id.ToString(),
                        Price = askWithExchange.ask.Price,
                        Amount = amounttobuy,
                        OrderType = OrderType.Buy
                    });

                    //Ce zelimo posodobiti količino in v primeru da je količina enaka 0 order odstranimo

                    askWithExchange.ask.Amount -= amounttobuy;
                    if (askWithExchange.ask.Amount <= 0)
                    {
                        exchanges.Where(e => e.Id == askWithExchange.exchange.Id)
                        .FirstOrDefault()
                        ?.Asks
                        .Remove(askWithExchange.ask);
                    }
                }
            }


            if (amountFullfilled != amount)
            {
                if(userOrders.Count == 0)
                {
                    return (userOrders, UserInstructionCompletion.Not);
                }
                else
                {
                    return (userOrders, UserInstructionCompletion.Semi);
                }
            }

            return (userOrders,UserInstructionCompletion.Fully);

        }
    }
}
