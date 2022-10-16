using Services;

namespace PetroineosIntradayReport.Core.Models
{
    public class PowerPosition
    {
        public List<PowerPeriod> Periods;

        public PowerPosition()
        {
            Periods = new List<PowerPeriod>();
            for (var i = 0; i < 24; i++)
            {
                Periods.Add(new PowerPeriod
                {
                    Period = i + 1,
                    Volume = 0
                });
            }
        }
    }
}
