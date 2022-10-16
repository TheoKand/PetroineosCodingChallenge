using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using PetroineosIntradayReport.Core;
using PetroineosIntradayReport.Core.Models;
using PetroineosIntradayReport.Service;

namespace PetroineosIntradayReport.Tests
{
    public class IntradayReportServiceTests
    {
        [Fact]
        public void GeneratesAndSavesIntradayReport()
        {
            //Arrange
            const string outputFolder = "someFolder";
            var powerPosition = new PowerPosition();
            var logger = new Mock<ILogger<WindowsService>>();
            var configuration = new Mock<IOptions<Configuration>>();
            configuration.SetupGet(c => c.Value.OutputFolder).Returns(outputFolder);
            var tradePositionsAggregator = new Mock<TradePositionsAggregator>();
            tradePositionsAggregator.Setup(t => t.AggregateForDay(It.IsAny<DateTime>())).Returns(powerPosition);
            var tradePositionsStore = new Mock<TradePositionsStore>();
            var nowDate = DateTime.Now;
            DateTime GetNowDate() => nowDate;

            //Act
            var sut = new IntradayReportService(logger.Object, configuration.Object, tradePositionsAggregator.Object, tradePositionsStore.Object, GetNowDate);
            sut.GenerateIntradayReport();

            //Assert
            tradePositionsAggregator.Verify(a => a.AggregateForDay(DateTime.Today), Times.Once);
            tradePositionsStore.Verify(s => s.Save(powerPosition, nowDate, outputFolder), Times.Once);

        }
    }
}
