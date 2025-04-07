using BDGCodingTask.Domain.Entities;
using BDGCodingTask.Domain.Enums;
using BDGCodingTask.Services;

namespace BDGCodingTaskTesting
{
    public class UserInstructionsEdgeTestCases
    {
        [Fact]
        public void BuyOrderExceedsAvailableLiquidityTest()
        {
            List<Exchange> exchanges = new List<Exchange>
    {
        new Exchange
        {
            Id = Guid.NewGuid(),
            Name = "Exchange1",
            Asks = new List<Order>
            {
                new Order { Price = 25000, Amount = 3 },
                new Order { Price = 26000, Amount = 2 },
                new Order { Price = 27000, Amount = 2 },
                new Order { Price = 28000, Amount = 2 }
            },
            EURBalance = 0,
            BTCBalance = 5
        }
    };

            var service = new UserInstructionsService();
            var result = service.GetUserInstructions("Buy", 10, exchanges);

            Assert.Equal(2, result.Item1.Count);
            Assert.Equal(3, result.Item1[0].Amount);
            Assert.Equal(2, result.Item1[1].Amount);

            Assert.Equal(UserInstructionCompletion.Semi, result.Item2);

            Assert.Equal(0, exchanges[0].BTCBalance);
            Assert.Equal(3 * 25000 + 2 * 26000, exchanges[0].EURBalance);
        }

        [Fact]
        public void SellOrderExceedsAvailableLiquidityTest()
        {
            List<Exchange> exchanges = new List<Exchange>
    {
        new Exchange
        {
            Id = Guid.NewGuid(),
            Name = "Exchange1",
            Bids = new List<Order>
            {
                new Order { Price = 27000, Amount = 4 },
                new Order { Price = 26000, Amount = 3 }
            },
            EURBalance = 300000,
            BTCBalance = 0
        }
    };

            var service = new UserInstructionsService();
            var result = service.GetUserInstructions("Sell", 10, exchanges);

            Assert.Equal(2, result.Item1.Count);
            Assert.Equal(4, result.Item1[0].Amount);
            Assert.Equal(3, result.Item1[1].Amount);

            Assert.Equal(7, exchanges[0].BTCBalance);
            Assert.Equal(300000 - (4 * 27000 + 3 * 26000), exchanges[0].EURBalance);
        }

        

        [Fact]
        public void SellPartiallyFillsSingleBidDueToEURLimitTest()
        {
            List<Exchange> exchanges = new List<Exchange>
    {
        new Exchange
        {
            Id = Guid.NewGuid(),
            Name = "Exchange1",
            Bids = new List<Order>
            {
                new Order { Price = 27000, Amount = 5 }
            },
            EURBalance = 40000,
            BTCBalance = 0
        }
    };

            var service = new UserInstructionsService();
            var result = service.GetUserInstructions("Sell", 5, exchanges); 

            Assert.Single(result.Item1);
            var executedOrder = result.Item1[0];

            Assert.Equal(40000m / 27000m,result.Item1[0].Amount);
            Assert.Equal(0, exchanges[0].EURBalance);
        }


    }


}