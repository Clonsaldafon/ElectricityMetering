using ElectricityMetering.Core.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ElectricityMetering.Core.Controllers
{
    public class IndicationsController : Controller
    {
        public List<List<string>> InfoData { get; set; } = new List<List<string>>();
        public List<List<string>> IndicationsNow { get; set; } = new List<List<string>>();
        public List<List<string>> IndicationsOneYearAgo { get; set; } = new List<List<string>>();
        public List<List<string>> IndicationsTwoYearsAgo { get; set; } = new List<List<string>>();

        public IndicationsController()
        {
            int year = DateTime.Now.Year;

            FillData();
        }

        private void FillData()
        {
            List<Garage> garages = Repository.GetAllGarages();
            List<string> blocksOfGarages = SplitAllBlockOfGarages();

            for (int row = 0; row < blocksOfGarages.Count; row++)
            {
                Garage garage = garages.First(g => g.Number == ParseBlockOfGarages(blocksOfGarages[row])[0]);

                List<string> rowDataInfo = new List<string>()
                {
                    blocksOfGarages[row],
                    garage.Owner.Name,
                    garage.Counter.Number,
                    garage.Seal.Number,
                    garage.Seal.Date.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture)
                };

                InfoData.Add(rowDataInfo);

                for (int i = 0; i < blocksOfGarages.Count; i++)
                {
                    List<string> rowDataIndicationsNow = new List<string>();
                    List<string> rowDataIndicationsOneYearAgo = new List<string>();
                    List<string> rowDataIndicationsTwoYearsAgo = new List<string>();

                    for (int j = 0; j < garage.Counter.IndicationsNow.Length; j++)
                    {
                        rowDataIndicationsNow.Add(garage.Counter.IndicationsNow[j].ToString());
                        rowDataIndicationsOneYearAgo.Add(garage.Counter.IndicationsOneYearAgo[j].ToString());
                        rowDataIndicationsTwoYearsAgo.Add(garage.Counter.IndicationsTwoYearsAgo[j].ToString());
                    }

                    IndicationsNow.Add(rowDataIndicationsNow);
                    IndicationsOneYearAgo.Add(rowDataIndicationsOneYearAgo);
                    IndicationsTwoYearsAgo.Add(rowDataIndicationsTwoYearsAgo);
                }
            }
        }

        public async Task SaveDataAsync(Dictionary<string, List<string>> indicationsByCounterNumber)
        {
            foreach (KeyValuePair<string, List<string>> keyValuePair in indicationsByCounterNumber)
            {
                Counter? counter = await Repository.GetCounterAsync(keyValuePair.Key);

                if (counter == null)
                {
                    return;
                }

                int[] indications = new int[keyValuePair.Value.Count];

                for (int i = 0; i < keyValuePair.Value.Count; i++)
                {
                    if (int.TryParse(keyValuePair.Value[i], out int indication))
                    {
                        indications[i] = indication;
                    }
                }

                await Repository.SaveIndicationsAsync(counter, indications);
            }
        }
    }
}
