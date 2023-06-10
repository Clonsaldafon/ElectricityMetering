using ElectricityMetering.Core.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricityMetering.Core.Controllers
{
    public class Controller
    {
        public string BlockOfGarages { get; set; }

        protected Garage? _garage;
        protected Owner? _owner;
        protected Counter? _counter;
        protected Seal? _seal;

        protected List<Garage> _garages = new List<Garage>();

        public Controller()
        {
            _garages = Repository.GetAllGarages();
        }

        /// <summary>
        /// Saves a new garage by garage number.
        /// </summary>
        /// <param name="garageNumber">Number of current garage.</param>
        /// <returns></returns>
        public async Task SaveGarageAsync(string garageNumber)
        {
            if (int.TryParse(garageNumber, out int number))
            {
                Garage garage = await Repository.GetGarageAsync(number);
                await Repository.SaveGarageAsync(garage, _owner, _counter, _seal);
            }
        }

        /// <summary>
        /// Save garage data.
        /// </summary>
        /// <param name="garage">Current garage.</param>
        /// <returns></returns>
        public async Task SaveGarageAsync(Garage garage)
        {
            await Repository.SaveGarageAsync(garage, _owner, _counter, _seal);
        }

        /// <summary>
        /// Checks whether it is possible to create a garage.
        /// </summary>
        /// <param name="garageNumber">Number of current garage.</param>
        /// <returns>Is it possible or not.</returns>
        public async Task<bool> CanCreateGarageAsync(int garageNumber)
        {
            return !(await GarageAlreadyExistsAsync(garageNumber));
        }

        /// <summary>
        /// Creates a new garage by its number.
        /// </summary>
        /// <param name="garageNumber">Number of current garage.</param>
        /// <returns></returns>
        public async Task CreateGarageAsync(int garageNumber)
        {
            if (await CanCreateGarageAsync(garageNumber))
            {
                _garage = await Repository.CreateGarageAsync(garageNumber);
                _garage = await Repository.SaveGarageAsync(_garage, _owner, _counter, _seal);

                FillDataByGarage();
            }
        }

        /// <summary>
        /// Loads the garage by number.
        /// </summary>
        /// <param name="garageNumber">Number of current garage.</param>
        /// <returns>Loaded garage.</returns>
        public async Task<Garage?> LoadGarageAsync(int garageNumber)
        {
            _garage = await Repository.GetGarageAsync(garageNumber);
            
            if (_garage != null)
            {
                FillDataByGarage();
            }

            return _garage;
        }

        private async Task<bool> GarageAlreadyExistsAsync(int garageNumber)
        {
            return await Repository.GetGarageAsync(garageNumber) != null;
        }

        private void FillDataByGarage()
        {
            _owner = _garage.Owner;
            _counter = _garage.Counter;
            _seal = _garage.Seal;
            _garages = Repository.GetBlockOfGarages(_garage);
        }

        /// <summary>
        /// Checks whether it is possible to create an owner.
        /// </summary>
        /// <param name="ownerName">Name of current owner.</param>
        /// <returns>Is it possible or not.</returns>
        public async Task<bool> CanCreateOwnerAsync(string ownerName)
        {
            return !string.IsNullOrEmpty(ownerName) && !(await OwnerAlreadyExistsAsync(ownerName));
        }

        /// <summary>
        /// Creates a new owner. If one already exists, it loads it from the database.
        /// </summary>
        /// <param name="ownerName">Name of current owner.</param>
        /// <returns></returns>
        public async Task CreateOwnerAsync(string ownerName)
        {
            if (await CanCreateOwnerAsync(ownerName))
            {
                _owner = await Repository.CreateOwnerAsync(ownerName);
            }
            else
            {
                _owner = await Repository.GetOwnerAsync(ownerName);
            }
        }

        private async Task<bool> OwnerAlreadyExistsAsync(string ownerName)
        {
            return await Repository.GetOwnerAsync(ownerName) != null;
        }

        /// <summary>
        /// Checks whether it is possible to create a counter.
        /// </summary>
        /// <param name="counterNumber">Number of current counter.</param>
        /// <returns>Is it possible or not.</returns>
        public async Task<bool> CanCreateCounterAsync(string counterNumber)
        {
            return !string.IsNullOrEmpty(counterNumber) && !(await CounterAlreadyExistsAsync(counterNumber));
        }

        /// <summary>
        /// Creates a new counter. If one already exists, it loads it from the database.
        /// </summary>
        /// <param name="counterNumber">Number of current counter.</param>
        /// <returns></returns>
        public async Task CreateCounterAsync(string counterNumber)
        {
            if (await CanCreateCounterAsync(counterNumber))
            {
                _counter = await Repository.CreateCounterAsync(counterNumber);
            }
            else
            {
                _counter = await Repository.GetCounterAsync(counterNumber);
            }            
        }

        private async Task<bool> CounterAlreadyExistsAsync(string counterNumber)
        {
            return await Repository.GetCounterAsync(counterNumber) != null;
        }

        /// <summary>
        /// Checks whether it is possible to create a seal.
        /// </summary>
        /// <param name="sealNumber">Number of current seal.</param>
        /// <param name="dateString">Date of sealing.</param>
        /// <returns>Is it possible or not.</returns>
        public async Task<bool> CanCreateSealAsync(string sealNumber, string dateString)
        {
            return !string.IsNullOrEmpty(sealNumber) && !(await SealAlreadyExistsAsync(sealNumber)) && DateOnly.TryParse(dateString, out _);
        }

        /// <summary>
        /// Creates a new seal. If one already exists, it loads it from the database.
        /// </summary>
        /// <param name="sealNumber">Number of current seal.</param>
        /// <param name="dateString">Date of sealing.</param>
        /// <returns></returns>
        public async Task CreateSealAsync(string sealNumber, string dateString)
        {
            if (await CanCreateSealAsync(sealNumber, dateString))
            {
                if (DateOnly.TryParse(dateString, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly date))
                {
                    _seal = await Repository.CreateSealAsync(sealNumber, DateOnly.Parse(dateString));
                }
            }
            else
            {
                _seal = await Repository.GetSealAsync(sealNumber);
            }
        }

        private async Task<bool> SealAlreadyExistsAsync(string sealNumber)
        {
            return await Repository.GetSealAsync(sealNumber) != null;
        }

        /// <summary>
        /// Gets information about the owner from the database.
        /// </summary>
        /// <param name="garageNumber">Number of current garage.</param>
        /// <returns>Loaded list of information.</returns>
        public async Task<List<string>> GetOwnerInfoAsync(int garageNumber)
        {
            Garage? garage = await Repository.GetGarageAsync(garageNumber);

            if (garage == null)
            {
                return new List<string>();
            }

            _owner = garage.Owner;

            return new List<string> { _owner.Name, _owner.Balance.ToString() };
        }

        /// <summary>
        /// Gets information about the counter from the database.
        /// </summary>
        /// <param name="garageNumber">Number of current garage.</param>
        /// <returns>Loaded list of information.</returns>
        public async Task<List<string>> GetCounterInfoAsync(int garageNumber)
        {
            Garage? garage = await Repository.GetGarageAsync(garageNumber);

            if (garage == null)
            {
                return new List<string>();
            }

            _counter = garage.Counter;

            return new List<string> { _counter.Number };
        }

        /// <summary>
        /// Gets information about the seal from the database.
        /// </summary>
        /// <param name="garageNumber">Number of current garage.</param>
        /// <returns>Loaded list of information.</returns>
        public async Task<List<string>> GetSealInfoAsync(int garageNumber)
        {
            Garage? garage = await Repository.GetGarageAsync(garageNumber);

            if (garage == null)
            {
                return new List<string>();
            }

            _seal = garage.Seal;

            return new List<string> { _seal.Number, _seal.Date.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture) };
        }

        /// <summary>
        /// Makes a string of garage numbers by owner separated by commas or hyphens.
        /// </summary>
        /// <param name="owner">Current owner.</param>
        /// <returns>The resulting string.</returns>
        public string SplitBlockOfGarages(Owner owner)
        {
            List<Garage> garages = GetGarages(owner);

            if (garages.Count == 0 && HasGarages(owner))
            {
                return GetFirstGarageNumber(owner);
            }

            return JoinGarageNumbers(garages);
        }

        private List<Garage> GetGarages(Owner owner)
        {
            return Repository.GetBlockOfGarages(owner);
        }

        private bool HasGarages(Owner owner)
        {
            return owner != null && owner.Garages.Count > 0;
        }

        private string GetFirstGarageNumber(Owner owner)
        {
            if (HasGarages(owner))
            {
                return owner.Garages[0].Number.ToString();
            }

            return string.Empty;
        }

        private string JoinGarageNumbers(List<Garage> garages)
        {
            StringBuilder result = new StringBuilder();

            int start = -1;
            for (int i = 0; i < garages.Count; i++)
            {
                if (start == -1)
                {
                    start = garages[i].Number;
                    result.Append(start);
                }
                else if (garages[i].Number == garages[i - 1].Number + 1)
                {
                    continue;
                }
                else
                {
                    if (start != garages[i - 1].Number)
                    {
                        if (garages[i - 1].Number - start == 1)
                        {
                            result.Append(',');
                        }
                        else
                        {
                            result.Append('-');
                        }
                        result.Append(garages[i - 1].Number);
                    }
                    result.Append(',');
                    start = garages[i].Number;
                    result.Append(start);
                }
            }
            if (start != -1 && start != garages[garages.Count - 1].Number)
            {
                if (garages[garages.Count - 1].Number - start == 1)
                {
                    result.Append(',');
                }
                else
                {
                    result.Append('-');
                }
                result.Append(garages[garages.Count - 1].Number);
            }

            return result.ToString();
        }

        /// <summary>
        /// Makes a string of garage numbers by garage number separated by commas or hyphens.
        /// </summary>
        /// <param name="garageNumber">Number of current garage.</param>
        /// <returns>The resulting string.</returns>
        public async Task<string> SplitBlockOfGarageAsync(int garageNumber)
        {
            Garage? garage = await GetGarageAsync(garageNumber);

            if (!HasGarage(garage))
            {
                return "Error 404";
            }

            List<Garage> garages = GetGaragesInLine(garage);

            if (garages.Count == 0)
            {
                return garage.Number.ToString();
            }

            BlockOfGarages = JoinGarageNumbers(garages);

            return BlockOfGarages;
        }

        private async Task<Garage?> GetGarageAsync(int garageNumber)
        {
            return await Repository.GetGarageAsync(garageNumber);
        }

        private bool HasGarage(Garage? garage)
        {
            return garage != null;
        }

        private List<Garage> GetGaragesInLine(Garage garage)
        {
            return Repository.GetBlockOfGarages(garage.Owner);
        }

        /// <summary>
        /// Makes a list of string of garage numbers separated by commas or hyphens.
        /// </summary>
        /// <returns>The resulting list of string.</returns>
        public List<string> SplitAllBlockOfGarages()
        {
            List<string> blocksOfGarages = new List<string>();

            foreach (Garage garage in _garages)
            {
                string blockOfGarages = SplitBlockOfGarages(garage.Owner);

                if (!blocksOfGarages.Contains(blockOfGarages))
                {
                    blocksOfGarages.Add(blockOfGarages);
                }
            }

            return blocksOfGarages;
        }

        /// <summary>
        /// Splits the string into individual garage numbers and adds them to the garageNumbers list.
        /// </summary>
        /// <param name="blockOfGarages">String of block of garages.</param>
        /// <returns>The resulting list of garage numbers.</returns>
        public List<int> ParseBlockOfGarages(string blockOfGarages)
        {
            List<int> garageNumbers = new List<int>();

            foreach (string part in blockOfGarages.Split(','))
            {
                if (part.Contains('-'))
                {
                    string[] range = part.Split('-');

                    if (int.TryParse(range[0], out int start) &&
                        int.TryParse(range[1], out int end))
                    {
                        for (int i = start; i <= end; i++)
                        {
                            garageNumbers.Add(i);
                        }
                    }
                }
                else
                {
                    if (int.TryParse(part, out int number))
                    {
                        garageNumbers.Add(number);
                    }
                }
            }

            return garageNumbers;
        }

        /// <summary>
        /// Fills in the block of garages passed as a list of garage numbers.
        /// </summary>
        /// <param name="garageNumbers">List of garage number.</param>
        /// <returns></returns>
        public async Task FillBlockOfGaragesAsync(List<int> garageNumbers)
        {
            foreach (int garageNumber in garageNumbers)
            {
                Garage? garage = await Repository.GetGarageAsync(garageNumber);

                if (garage == null)
                {
                    garage = await Repository.CreateGarageAsync(garageNumber);
                }

                await SaveGarageAsync(garage);
                _garages.Add(garage);
            }
        }
    }
}
