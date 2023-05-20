using ElectricityMetering.Core.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricityMetering.Core.Controllers
{
    public class IndicationsController : Controller
    {
        public List<List<string>> Data { get; set; } = new List<List<string>>();

        public IndicationsController(int year)
        {
            FillData(year);
            FillData(year - 1);
            FillData(year - 2);
        }

        private void FillData(int year)
        {
            List<Garage> garages = _repository.GetAllGarages();
            List<string> blocksOfGarages = SplitAllBlockOfGarages();

            for (int i = 0; i < garages.Count; i++)
            {
                List<string> rowData = new List<string>()
                {
                    blocksOfGarages[i],
                    _repository.GetOwnerAsync(garages[i]).Result.Name,
                    _repository.GetCounterAsync(garages[i]).Result.Number,
                    _repository.GetSealAsync(garages[i]).Result.Number,
                    _repository.GetSealAsync(garages[i]).Result.Date.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture)
                };

                int nowYear = DateTime.Now.Year;

                if (year == nowYear)
                {
                    foreach (int indication in garages[i].Counter.IndicationsNow)
                    {
                        rowData.Add(indication.ToString());
                    }
                }
                else if (year == nowYear - 1)
                {
                    foreach (int indication in garages[i].Counter.IndicationsOneYearAgo)
                    {
                        rowData.Add(indication.ToString());
                    }
                }
                else if (year == nowYear - 2)
                {
                    foreach (int indication in garages[i].Counter.IndicationsTwoYearsAgo)
                    {
                        rowData.Add(indication.ToString());
                    }
                }

                Data.Add(rowData);
            }
        }
    }
}
