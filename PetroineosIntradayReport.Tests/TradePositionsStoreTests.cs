using PetroineosIntradayReport.Core;
using PetroineosIntradayReport.Core.Models;
using Services;

namespace PetroineosIntradayReport.Tests
{
    public class TradePositionsStoreTests : IDisposable
    {

        private readonly string _outputFolder;
        public TradePositionsStoreTests()
        {
            _outputFolder = CreateTempOutputFolder();
        }


        [Fact]
        public void SavesCsvFile()
        {
            //Arrange
            var powerPosition = CreatePowerPosition();
            var extractTime = DateTime.Now;

            //Act
            var sut = new TradePositionsStore();
            sut.Save(powerPosition, extractTime, _outputFolder);

            //Assert
            var expectedFilename = Path.Combine(_outputFolder, $"PowerPosition_{extractTime:yyyyMMdd_HHmm}.csv");

            Assert.True(Directory.Exists(_outputFolder));
            Assert.True(File.Exists(expectedFilename));

        }

        private static PowerPosition CreatePowerPosition()
        {
            var result = new PowerPosition();
            for (var i = 0; i < 24; i++)
            {
                var powerPeriod = new PowerPeriod { Period = i + 1, Volume = 1 };
                result.Periods.Add(powerPeriod);
            }
            return result;
        }

        private static string CreateTempOutputFolder()
        {
            var tempDirectory = Path.Combine(Path.GetTempPath(), nameof(TradePositionsStoreTests));
            if (!Directory.Exists(tempDirectory))
            {
                Directory.CreateDirectory(tempDirectory);
            }
            return tempDirectory;
        }

        public void Dispose()
        {
            if (Directory.Exists(_outputFolder))
            {
                Directory.Delete(_outputFolder, true);
            }
        }
    }
}
