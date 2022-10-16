using FluentAssertions;
using Moq;
using PetroineosIntradayReport.Core;
using Services;

namespace PetroineosIntradayReport.Tests
{
    public class TradePositionsAggregatorTests
    {
        [Fact]
        public void AggregatesTradePositions()
        {
            //Arrange
            const int howManyPowerTrades = 3;
            var mockPowerService = new Mock<IPowerService>();
            mockPowerService.Setup(p => p.GetTrades(It.IsAny<DateTime>())).Returns(CreatePowerTrades(howManyPowerTrades));

            //Act
            var sut = new TradePositionsAggregator(mockPowerService.Object);
            var powerPosition = sut.AggregateForDay(DateTime.Today);

            //Assert
            powerPosition.Periods.Should().HaveCount(24);
            powerPosition.Periods.ForEach(p => p.Volume.Should().Be(howManyPowerTrades));

        }

        [Fact]
        public void WithNegativeVolumes_AggregatesTradePositions()
        {
            //Arrange
            const int howManyPowerTrades = 3;
            var mockPowerService = new Mock<IPowerService>();
            mockPowerService.Setup(p => p.GetTrades(It.IsAny<DateTime>())).Returns(CreatePowerTrades(howManyPowerTrades, -1));

            //Act
            var sut = new TradePositionsAggregator(mockPowerService.Object);
            var powerPosition = sut.AggregateForDay(DateTime.Today);

            //Assert
            powerPosition.Periods.Should().HaveCount(24);
            powerPosition.Periods.ForEach(p => p.Volume.Should().Be(-howManyPowerTrades));
        }

        private IEnumerable<PowerTrade> CreatePowerTrades(int howMany, int sign = 1)
        {
            var result = new List<PowerTrade>();
            for (var i = 0; i < howMany; i++)
            {
                result.Add(CreatePowerTradeWhereVolumeIsAlwaysOne(sign));
            }
            return result;
        }

        private PowerTrade CreatePowerTradeWhereVolumeIsAlwaysOne(int sign = 1)
        {
            var result = PowerTrade.Create(DateTime.Now, 24);
            result.Periods.ToList().ForEach(p => p.Volume = sign * 1);
            return result;
        }

    }
}
