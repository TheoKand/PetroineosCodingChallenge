using PetroineosIntradayReport.Core.Models;
using System.Text;

namespace PetroineosIntradayReport.Core
{
    public class TradePositionsStore
    {
        public TradePositionsStore() {}

        public virtual void Save(PowerPosition powerPosition, DateTime extractTime, string outputFolder)
        {
            var file = new StringBuilder();
            file.AppendLine("Local Time,Volume");
            foreach (var period in powerPosition.Periods)
            {
                file.AppendLine($"{GetTimeFromPeriod(period.Period)},{period.Volume}");
            }

            var fullPath = Path.Combine(outputFolder, GetFilename(extractTime));
            File.WriteAllText(fullPath, file.ToString());

        }

        private static string GetTimeFromPeriod(int period)
        {
            var hour = period == 1 ? 23 : period - 2;
            var time = $"{hour:00}:00";
            return time;
        }

        protected string GetFilename(DateTime extractTime)
        {
            var fileName = $"PowerPosition_{extractTime:yyyyMMdd_HHmm}.csv";
            return fileName;
        }
    }
}
