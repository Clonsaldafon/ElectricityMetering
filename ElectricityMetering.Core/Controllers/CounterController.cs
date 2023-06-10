using ElectricityMetering.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricityMetering.Core.Controllers
{
    public class CounterController
    {
        public int[] ConsumptionNow { get; set; } = new int[12];
        public int[] ConsumptionOneYearAgo { get; set; } = new int[12];
        public int[] ConsumptionTwoYearsAgo { get; set; } = new int[12];

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
