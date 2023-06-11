using ElectricityMetering.Core.Models;

namespace ElectricityMetering.Core.Controllers
{
    public class CounterController
    {
        public int[] ConsumptionNow { get; set; } = new int[12];
        public int[] ConsumptionOneYearAgo { get; set; } = new int[12];
        public int[] ConsumptionTwoYearsAgo { get; set; } = new int[12];

        /// <summary>
        /// Calculates consumption according to the indications in the counter.
        /// </summary>
        /// <param name="counter">Current counter.</param>
        public void CalculateConsumption(Counter counter)
        {
            int lastInd = counter.IndicationsNow.Length - 1;

            for (int i = 1; i < 12; i++)
            {
                ConsumptionTwoYearsAgo[i] = counter.IndicationsTwoYearsAgo[i] - counter.IndicationsTwoYearsAgo[i - 1];
                ConsumptionOneYearAgo[i] = counter.IndicationsOneYearAgo[i] - counter.IndicationsOneYearAgo[i - 1];
                ConsumptionNow[i] = counter.IndicationsNow[i] - counter.IndicationsNow[i - 1];
            }

            ConsumptionOneYearAgo[0] = counter.IndicationsOneYearAgo[0] - counter.IndicationsTwoYearsAgo[lastInd];
            ConsumptionNow[0] = counter.IndicationsNow[0] - counter.IndicationsOneYearAgo[lastInd];
        }
    }
}
