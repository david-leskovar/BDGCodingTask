using BDGCodingTask.Domain.Entities;
using BDGCodingTask.Domain.Enums;
using BDGCodingTask.Services.Interfaces;

namespace BDGCodingTask.Services
{
    public class UserInstructionsService : IUserInstructionsService
    {
        /*
         * param orderType: tip orderja, ki ga uporabnik želi oddati
         * param amount: količina, ki jo uporabnik želi oddati
         * param exchanges: seznam vseh exchange-ov
         * 
         * return: seznam UserInstruction-ov
         * 
         * */

        public (List<UserInstruction>,UserInstructionCompletion) GetUserInstructions(string orderType, decimal amount, List<Exchange> exchanges)
        {
            decimal amountFullfilled = 0;
            List<UserInstruction> userOrders = new List<UserInstruction>();

            if (orderType == "Sell")
            {
                //Dobimo vse bide iz vseh exchange-ov, in jih sortiramo po ceni(od najvišje do najnižje)
                var bidsWithExchange = exchanges
               .SelectMany(exchange => exchange.Bids, (exchange, bid) => new { exchange, bid })
               .OrderByDescending(order => order.bid.Price)
               .ToList();

                foreach (var bidWithExchange in bidsWithExchange)
                {

                    //Če je količina izpolnjena prekinemo loop
                    if (amountFullfilled >= amount)
                    {
                        break;
                    }

                    //Izračunamo količino, ki jo lahko v tem bidu prodamo
                    //Najprej gledamo minimalno vrednost med celotno količino, ki je na volju v bidu in količino, ki jo je še potrebno izpolniti
                    //Nato gledamo še največjo količino, ki jo exchange lahko kupi in izberemo minimalno vrednost med tema dvema
                    var amounttosell = Math.Min(Math.Min(bidWithExchange.bid.Amount, amount - amountFullfilled), bidWithExchange.exchange.EURBalance / bidWithExchange.bid.Price);


                    //Če je količina 0, gremo na naslednji bid
                    if (amounttosell == 0)
                    {
                        continue;
                    }

                    //Posodobimo stanje na exchangeu, prištejemo izpolnjeni količini in zmanjšamo količino v bidu
                    bidWithExchange.exchange.EURBalance -= amounttosell * bidWithExchange.bid.Price;
                    bidWithExchange.exchange.BTCBalance += amounttosell;
                    amountFullfilled += amounttosell;
                    bidWithExchange.bid.Amount -= amounttosell;


                    //Dodamo nov UserInstruction
                    userOrders.Add(new UserInstruction
                    {
                        Id = Guid.NewGuid(),
                        ExchangeId = bidWithExchange.exchange.Id.ToString(),
                        Price = bidWithExchange.bid.Price,
                        Amount = amounttosell,
                        OrderType = OrderType.Sell
                    });


                    //V primeru da je bil celoten bid izpolnjen, ga odstranimo iz exchangea
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

                //Dobimo vse aske iz vseh exchange-ov in jih sortiramo po ceni(od najnižje do najvišje)
                var asksWithExchange = exchanges
               .SelectMany(exchange => exchange.Asks, (exchange, ask) => new { exchange, ask })
               .OrderBy(order => order.ask.Price)
               .ToList();

                foreach (var askWithExchange in asksWithExchange)
                {

                    //Če je količina izpolnjena prekinemo loop
                    if (amountFullfilled >= amount)
                    {
                        break;
                    }

                    //Izračunamo količino, ki jo lahko v tem asku kupimo
                    //Najprej gledamo minimalno vrednost med celotno količino, ki je na volju v asku in količino, ki jo je še potrebno izpolniti
                    //Nato gledamo še največjo količino, ki jo exchange lahko proda in izberemo minimalno vrednost med tema dvema
                    var amounttobuy = Math.Min(Math.Min(askWithExchange.ask.Amount, amount - amountFullfilled), askWithExchange.exchange.BTCBalance);

                    //Če je količina 0, gremo na naslednji ask
                    if (amounttobuy == 0)
                    {
                        continue;
                    }


                    //Posodobimo stanje na exchangeu, prištejemo izpolnjeni količini in zmanjšamo količino v asku
                    askWithExchange.exchange.EURBalance += amounttobuy * askWithExchange.ask.Price;
                    askWithExchange.exchange.BTCBalance -= amounttobuy;
                    amountFullfilled += amounttobuy;
                    askWithExchange.ask.Amount -= amounttobuy;



                    //Dodamo nov UserInstruction
                    userOrders.Add(new UserInstruction
                    {
                        Id = Guid.NewGuid(),
                        ExchangeId = askWithExchange.exchange.Id.ToString(),
                        Price = askWithExchange.ask.Price,
                        Amount = amounttobuy,
                        OrderType = OrderType.Buy
                    });



                    //V primeru da je bil celoten ask izpolnjen, ga odstranimo iz exchange-a
                    if (askWithExchange.ask.Amount <= 0)
                    {
                        exchanges.Where(e => e.Id == askWithExchange.exchange.Id)
                        .FirstOrDefault()
                        ?.Asks
                        .Remove(askWithExchange.ask);
                    }
                }
            }


            //Glede na izpolnjeno količino, vrnemo Listo in UserInstructionCompletion

            if (amountFullfilled != amount)
            {
                if (userOrders.Count == 0)
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
