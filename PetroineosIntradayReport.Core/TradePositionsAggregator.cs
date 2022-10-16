using PetroineosIntradayReport.Core.Models;
using Services;

namespace PetroineosIntradayReport.Core
{
    public class TradePositionsAggregator
    {
        private readonly IPowerService? _powerService;

        public TradePositionsAggregator() { }

        public TradePositionsAggregator(IPowerService powerService)
        {
            _powerService = powerService;
        }

        public virtual PowerPosition AggregateForDay(DateTime date)
        {
            var result = new PowerPosition();

            var trades = _powerService.GetTrades(date);

            foreach (var trade in trades)
            {
                foreach (var period in trade.Periods)
                {
                    result.Periods.Single(p => p.Period == period.Period).Volume += period.Volume;
                }
            }
            return result;
        }
    }
}
