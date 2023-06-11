using ElectricityMetering.Core.Models;
using System.Globalization;
using Newtonsoft.Json.Linq;

namespace ElectricityMetering.Core.Controllers
{
    public class IndicationsController : Controller
    {
        public List<List<string>> InfoData { get; set; } = new List<List<string>>();
        public List<List<string>> IndicationsNow { get; set; } = new List<List<string>>();
        public List<List<string>> IndicationsOneYearAgo { get; set; } = new List<List<string>>();
        public List<List<string>> IndicationsTwoYearsAgo { get; set; } = new List<List<string>>();

        public bool YearChanged { get; set; }

        private OwnerController _ownerController = new OwnerController();

        public IndicationsController()
        {
            string filePathConfigJSON = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName + "\\AppSettings\\config.json";

            if (File.Exists(filePathConfigJSON))
            {
                JObject json = JObject.Parse(File.ReadAllText(filePathConfigJSON));
                int lastYear = (int)json["LastYear"];
                int currentYear = DateTime.Now.Year;

                YearChanged = currentYear > lastYear;

                if (YearChanged)
                {
                    json["LastYear"] = currentYear;
                    File.WriteAllText(filePathConfigJSON, json.ToString());

                    _ = ChangeYearsAsync();
                }
            }

            FillData();
        }

        private async Task ChangeYearsAsync()
        {
            List<Counter> counters = Repository.GetCounters();

            foreach (Counter counter in counters)
            {
                counter.IndicationsTwoYearsAgo = counter.IndicationsOneYearAgo;
                counter.IndicationsOneYearAgo = counter.IndicationsNow;
                counter.IndicationsNow = new int[12];

                await Repository.SaveCounterAsync(counter);
            }
        }

        private void FillData()
        {
            List<string> blocksOfGarages = SplitAllBlockOfGarages();

            for (int row = 0; row < blocksOfGarages.Count; row++)
            {
                List<int> garages = ParseBlockOfGarages(blocksOfGarages[row]);

                if (garages.Count  == 0)
                {
                    continue;
                }

                Garage garage = _garages.First(g => g.Number == garages[0]);

                List<string> rowDataInfo = new List<string>()
                {
                    blocksOfGarages[row],
                    garage.Owner.Name,
                    garage.Counter.Number,
                    garage.Seal.Number,
                    garage.Seal.Date.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture)
                };

                InfoData.Add(rowDataInfo);

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

                _ = _ownerController.UpdateBalanceAsync(garage.Owner, garage.Counter);
            }
        }

        /// <summary>
        /// Saves the indications entered in the table.
        /// </summary>
        /// <param name="indicationsByCounterNumber">Entered indications by counter number.</param>
        /// <returns>Successfully or not.</returns>
        public async Task<bool> SaveDataAsync(Dictionary<string, List<string>> indicationsByCounterNumber)
        {
            foreach (KeyValuePair<string, List<string>> keyValuePair in indicationsByCounterNumber)
            {
                Counter? counter = await Repository.GetCounterAsync(keyValuePair.Key);

                if (counter == null)
                {
                    return false;
                }

                int[] indications = new int[keyValuePair.Value.Count];

                for (int i = 0; i < keyValuePair.Value.Count; i++)
                {
                    if (int.TryParse(keyValuePair.Value[i], out int indication))
                    {
                        indications[i] = indication;
                    }
                    else
                    {
                        return false;
                    }
                }

                await Repository.SaveIndicationsAsync(counter, indications);

                Garage? garage = await Repository.GetGarageAsync(counter);

                if (garage != null)
                {
                    Owner owner = await Repository.GetOwnerAsync(garage);
                    await _ownerController.UpdateBalanceAsync(owner, counter);
                }
            }

            return true;
        }
    }
}
