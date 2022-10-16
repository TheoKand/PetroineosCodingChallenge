using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using PetroineosIntradayReport.Core;
using Services;

namespace PetroineosIntradayReport.Service
{
    public class IntradayReportService
    {
        private readonly ILogger<WindowsService> _logger;
        private readonly IOptions<Configuration> _configuration;
        private readonly TradePositionsAggregator _tradePositionsAggregator;
        private readonly TradePositionsStore _tradePositionsStore;
        private readonly Func<DateTime> _now;

        public IntradayReportService(ILogger < WindowsService> logger, IOptions<Configuration> configuration, TradePositionsAggregator aggregator,TradePositionsStore store,Func<DateTime>? now = null)
        {
            _now = now ?? new Func<DateTime>(() => DateTime.Now);
            _logger = logger;
            _configuration = configuration;
            _tradePositionsAggregator = aggregator;
            _tradePositionsStore = store;
        }

        public void GenerateIntradayReport()
        {
            try
            {
                _logger.LogInformation("Aggregating trade positions...");
                var dayAheadPowerPosition = _tradePositionsAggregator.AggregateForDay(DateTime.Today);

                _logger.LogInformation("Saving intraday report...");
                _tradePositionsStore.Save(dayAheadPowerPosition, _now(), _configuration.Value.OutputFolder!);

                _logger.LogInformation("Finished...");

            }
            catch (PowerServiceException powerServiceException)
            {
                _logger.LogError(powerServiceException, "The Power Service is unavailable");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception in {nameof(GenerateIntradayReport)}");
            }

        }
    }
}
