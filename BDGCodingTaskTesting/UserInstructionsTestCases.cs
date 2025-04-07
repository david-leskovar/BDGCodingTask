using BDGCodingTask.Domain.Entities;
using BDGCodingTask.Domain.Enums;
using BDGCodingTask.Services;

namespace BDGCodingTaskTesting
{
    public class UserInstructionsTestCases
    {
        [Fact]
        public void BuyingBTCTest()
        {
           
            List<Exchange> exchanges = new List<Exchange>
            {
                new Exchange
                {
                    Id = Guid.NewGuid(),
                    Name = "Exchange1",
                    Asks = new List<Order>
                    {
                        new Order { Price = 25000, Amount = 20 },
                        new Order { Price = 26000, Amount = 30 },
                    },
                    EURBalance = 100000,
                    BTCBalance = 100
                }
            };

            var service = new UserInstructionsService();
            var result = service.GetUserInstructions("Buy",10,exchanges);

            Assert.Equal(UserInstructionCompletion.Fully, result.Item2);
            Assert.Equal(10, result.Item1[0].Amount=10);
            Assert.Single(result.Item1);
            Assert.Equal(90, exchanges[0].BTCBalance);
            Assert.Equal(100000 + 25000 * 10, exchanges[0].EURBalance);
        }

        [Fact]
        public void SellingBTCTest()
        {
            List<Exchange> exchanges = new List<Exchange>
            {
                new Exchange
                {
                    Id = Guid.NewGuid(),
                    Name = "Exchange1",
                    Bids = new List<Order>
                    {
                        new Order { Price = 28000, Amount = 10 },
                        new Order { Price = 27000, Amount = 10 }
                    },
                    EURBalance = 1000000,
                    BTCBalance = 50
                }
            };

            var service = new UserInstructionsService();
            var result = service.GetUserInstructions("Sell", 20, exchanges);

            Assert.Equal(UserInstructionCompletion.Fully, result.Item2);

            Assert.Equal(10, result.Item1[0].Amount);
            Assert.Equal(10, result.Item1[1].Amount);
            Assert.Equal(70, exchanges[0].BTCBalance);
            Assert.Equal(1000000 - (10 * 28000 + 10 * 27000), exchanges[0].EURBalance);      
        }

        [Fact]
        public void PartialBuyAcrossTwoAsksTest()
        {
            List<Exchange> exchanges = new List<Exchange>
            {
        new Exchange
        {
            Id = Guid.NewGuid(),
            Name = "Exchange1",
            Asks = new List<Order>
            {
                new Order { Price = 25000, Amount = 5 },
                new Order { Price = 26000, Amount = 10 },
            },
            EURBalance = 50000,
            BTCBalance = 100
        }
    };

            var service = new UserInstructionsService();
            var result = service.GetUserInstructions("Buy", 10, exchanges);

            Assert.Equal(2, result.Item1.Count);
            Assert.Equal(5, result.Item1[0].Amount);
            Assert.Equal(5, result.Item1[1].Amount);

            Assert.Equal(UserInstructionCompletion.Fully, result.Item2);


            Assert.Equal(5, exchanges[0].Asks.Where(x => x.Price == 26000).First().Amount);
            Assert.Equal(0, exchanges[0].Asks.Where(x => x.Price == 25000).Count());

            Assert.Equal(90, exchanges[0].BTCBalance);
            Assert.Equal(50000 + (5 * 25000 + 5 * 26000), exchanges[0].EURBalance);
        }

        [Fact]
        public void PartialSellDueToLimitedBidsTest()
        {
            List<Exchange> exchanges = new List<Exchange>
    {
        new Exchange
        {
            Id = Guid.NewGuid(),
            Name = "Exchange1",
            Bids = new List<Order>
            {
                new Order { Price = 29000, Amount = 5 }
            },
            EURBalance = 200000,
            BTCBalance = 0
        }
    };

            var service = new UserInstructionsService();
            var result = service.GetUserInstructions("Sell", 10, exchanges);

            Assert.Single(result.Item1);
            Assert.Equal(5, result.Item1[0].Amount);
            Assert.Equal(UserInstructionCompletion.Semi, result.Item2);

            Assert.Equal(5, exchanges[0].BTCBalance);
            Assert.Equal(200000 - 5 * 29000, exchanges[0].EURBalance);
        }

        [Fact]
        public void MultiExchangeBuyTest()
        {
            List<Exchange> exchanges = new List<Exchange>
    {
        new Exchange
        {
            Id = Guid.NewGuid(),
            Name = "Exchange1",
            Asks = new List<Order> { new Order { Price = 24000, Amount = 5 } },
            EURBalance = 0,
            BTCBalance = 10
        },
        new Exchange
        {
            Id = Guid.NewGuid(),
            Name = "Exchange2",
            Asks = new List<Order> { new Order { Price = 25000, Amount = 10 } },
            EURBalance = 0,
            BTCBalance = 10
        }
    };

            var service = new UserInstructionsService();
            var result = service.GetUserInstructions("Buy", 10, exchanges);

            Assert.Equal(2, result.Item1.Count);
            Assert.Equal(5, result.Item1[0].Amount); 
            Assert.Equal(5, result.Item1[1].Amount);
            Assert.Equal(UserInstructionCompletion.Fully, result.Item2);
            Assert.Equal(5, exchanges[0].BTCBalance);
            
            Assert.Equal(24000 * 5, exchanges[0].EURBalance);

            Assert.Equal(5, exchanges[1].BTCBalance);
            Assert.Equal(25000 * 5, exchanges[1].EURBalance);
        }


        [Fact]
        public void MultiExchangeSellTest()
        {
            List<Exchange> exchanges = new List<Exchange>
    {
        new Exchange
        {
            Id = Guid.NewGuid(),
            Name = "Exchange1",
            Bids = new List<Order> { new Order { Price = 28000, Amount = 5 } },
            EURBalance = 200000,
            BTCBalance = 0
        },
        new Exchange
        {
            Id = Guid.NewGuid(),
            Name = "Exchange2",
            Bids = new List<Order> { new Order { Price = 27000, Amount = 10 } },
            EURBalance = 300000,
            BTCBalance = 0
        }
    };

            var service = new UserInstructionsService();
            var result = service.GetUserInstructions("Sell", 10, exchanges);

            Assert.Equal(2, result.Item1.Count);
            Assert.Equal(5, result.Item1[0].Amount);
            Assert.Equal(5, result.Item1[1].Amount);

            Assert.Equal(UserInstructionCompletion.Fully, result.Item2);

            Assert.Equal(5, exchanges[0].BTCBalance);
            Assert.Equal(200000 - 5 * 28000, exchanges[0].EURBalance);

            Assert.Equal(5, exchanges[1].BTCBalance);
            Assert.Equal(300000 - 5 * 27000, exchanges[1].EURBalance);
        }


    }
}