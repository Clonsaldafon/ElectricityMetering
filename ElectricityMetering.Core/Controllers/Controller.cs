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

        public async Task SaveGarageAsync(string garageNumber)
        {
            if (int.TryParse(garageNumber, out int number))
            {
                Garage garage = await Repository.GetGarageAsync(number);
                await Repository.SaveGarageAsync(garage, _owner, _counter, _seal);
            }
        }

        public async Task SaveGarageAsync(Garage garage)
        {
            await Repository.SaveGarageAsync(garage, _owner, _counter, _seal);
        }

        public async Task<bool> CanCreateGarageAsync(int garageNumber) =>
            !(await GarageAlreadyExistsAsync(garageNumber));

        public async Task CreateGarageAsync(int garageNumber)
        {
            if (await CanCreateGarageAsync(garageNumber))
            {
                _garage = await Repository.CreateGarageAsync(garageNumber);
                _garage = await Repository.SaveGarageAsync(_garage, _owner, _counter, _seal);

                FillDataByGarage();
            }
        }

        public async Task<bool> CanLoadGarage(int garageNumber) =>
            await GarageAlreadyExistsAsync(garageNumber);

        public async Task<Garage?> LoadGarageAsync(int garageNumber)
        {
            _garage = await Repository.GetGarageAsync(garageNumber);
            
            if (_garage != null)
            {
                FillDataByGarage();
            }

            return _garage;
        }

        private async Task<bool> GarageAlreadyExistsAsync(int garageNumber) =>
            (await Repository.GetGarageAsync(garageNumber)) != null;

        protected void FillDataByGarage()
        {
            _owner = _garage.Owner;
            _counter = _garage.Counter;
            _seal = _garage.Seal;
            _garages = Repository.GetBlockOfGarages(_garage);
        }

        public async Task<bool> CanCreateOwnerAsync(string ownerName) =>
            !string.IsNullOrEmpty(ownerName) && !(await OwnerAlreadyExistsAsync(ownerName));

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

        public async Task<bool> CanCreateCounterAsync(string counterNumber) =>
            !string.IsNullOrEmpty(counterNumber) && !(await CounterAlreadyExistsAsync(counterNumber));

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

        private async Task<bool> CounterAlreadyExistsAsync(string counterNumber) =>
            await Repository.GetCounterAsync(counterNumber) != null;

        public async Task<bool> CanCreateSealAsync(string sealNumber, string dateString) =>
            !string.IsNullOrEmpty(sealNumber) && !(await SealAlreadyExistsAsync(sealNumber)) && DateOnly.TryParse(dateString, out _);

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

        private async Task<bool> SealAlreadyExistsAsync(string sealNumber) =>
            await Repository.GetSealAsync(sealNumber) != null;

        public async Task<List<string>> GetOwnerInfoAsync(int garageNumber)
        {
            if (_garage == null)
            {
                return new List<string>();
            }

            _owner = _garage.Owner;

            return new List<string> { _owner.Name, _owner.Balance.ToString() };
        }

        public async Task<List<string>> GetCounterInfoAsync(int garageNumber)
        {
            if (_garage == null)
            {
                return new List<string>();
            }

            _counter = _garage.Counter;

            return new List<string> { _counter.Number };
        }

        public async Task<List<string>> GetSealInfoAsync(int garageNumber)
        {
            if (_garage == null)
            {
                return new List<string>();
            }

            _seal = _garage.Seal;

            return new List<string> { _seal.Number, _seal.Date.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture) };
        }

        public string SplitBlockOfGarages(Owner owner)
        {
            List<Garage> garages = Repository.GetBlockOfGarages(owner);

            if (garages.Count == 0 && owner != null && owner.Garages.Count > 0)
            {
                return owner.Garages[0].Number.ToString();
            }

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

        public async Task<string> SplitBlockOfGarageAsync(int garageNumber)
        {
            if (_garage == null)
            {
                _garage = await Repository.GetGarageAsync(garageNumber);
            }

            if (_garage == null)
            {
                return "Error 404";
            }

            if (_garages.Count == 0)
            {
                return _garage.Number.ToString();
            }

            StringBuilder result = new StringBuilder();

            int start = -1;
            for (int i = 0; i < _garages.Count; i++)
            {
                if (start == -1)
                {
                    start = _garages[i].Number;
                    result.Append(start);
                }
                else if (_garages[i].Number == _garages[i - 1].Number + 1)
                {
                    continue;
                }
                else
                {
                    if (start != _garages[i - 1].Number)
                    {
                        if (_garages[i - 1].Number - start == 1)
                        {
                            result.Append(',');
                        }
                        else
                        {
                            result.Append('-');
                        }
                        result.Append(_garages[i - 1].Number);
                    }
                    result.Append(',');
                    start = _garages[i].Number;
                    result.Append(start);
                }
            }
            if (start != -1 && start != _garages[_garages.Count - 1].Number)
            {
                if (_garages[_garages.Count - 1].Number - start == 1)
                {
                    result.Append(',');
                }
                else
                {
                    result.Append('-');
                }
                result.Append(_garages[_garages.Count - 1].Number);
            }

            BlockOfGarages = result.ToString();

            return BlockOfGarages;
        }

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
