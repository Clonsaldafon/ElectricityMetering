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
        protected readonly Repository _repository = new Repository();

        protected Garage? _garage;
        protected Owner? _owner;
        protected Counter? _counter;
        protected Seal? _seal;

        protected List<Garage> _garages = new List<Garage>();

        public async Task SaveGarageAsync(int garageNumber)
        {
            Garage garage = await _repository.GetGarageAsync(garageNumber);
            await _repository.SaveGarageAsync(garage, _owner, _counter, _seal);
        }

        public async Task SaveGarageAsync(Garage garage)
        {
            await _repository.SaveGarageAsync(garage, _owner, _counter, _seal);
        }

        public async Task<bool> CanCreateGarageAsync(int garageNumber) =>
            !(await GarageAlreadyExistsAsync(garageNumber));

        public async Task CreateGarageAsync(int garageNumber)
        {
            if (await CanCreateGarageAsync(garageNumber))
            {
                _garage = await _repository.CreateGarageAsync(garageNumber);
                _garage = await _repository.SaveGarageAsync(_garage, _owner, _counter, _seal);

                FillDataByGarage();
            }
        }

        public async Task<bool> CanLoadGarage(int garageNumber) =>
            await GarageAlreadyExistsAsync(garageNumber);

        public async Task<Garage?> LoadGarageAsync(int garageNumber)
        {
            _garage = await _repository.GetGarageAsync(garageNumber);
            
            if (_garage != null)
            {
                FillDataByGarage();
            }

            return _garage;
        }

        private async Task<bool> GarageAlreadyExistsAsync(int garageNumber) =>
            (await _repository.GetGarageAsync(garageNumber)) != null;

        protected void FillDataByGarage()
        {
            _owner = _garage.Owner;
            _counter = _garage.Counter;
            _seal = _garage.Seal;
            _garages = _repository.GetBlockOfGarages(_garage);
        }

        public async Task<bool> CanCreateOwnerAsync(string ownerName) =>
            !string.IsNullOrEmpty(ownerName) && !(await OwnerAlreadyExistsAsync(ownerName));

        public async Task CreateOwnerAsync(string ownerName)
        {
            if (await CanCreateOwnerAsync(ownerName))
            {
                _owner = await _repository.CreateOwnerAsync(ownerName);
            }
            else
            {
                _owner = await _repository.GetOwnerAsync(ownerName);
            }
        }

        private async Task<bool> OwnerAlreadyExistsAsync(string ownerName)
        {
            return await _repository.GetOwnerAsync(ownerName) != null;
        }

        public async Task<bool> CanCreateCounterAsync(string counterNumber) =>
            !string.IsNullOrEmpty(counterNumber) && !(await CounterAlreadyExistsAsync(counterNumber));

        public async Task CreateCounterAsync(string counterNumber)
        {
            if (await CanCreateCounterAsync(counterNumber))
            {
                _counter = await _repository.CreateCounterAsync(counterNumber);
            }
            else
            {
                _counter = await _repository.GetCounterAsync(counterNumber);
            }            
        }

        private async Task<bool> CounterAlreadyExistsAsync(string counterNumber) =>
            await _repository.GetCounterAsync(counterNumber) != null;

        public async Task<bool> CanCreateSealAsync(string sealNumber, string dateString) =>
            !string.IsNullOrEmpty(sealNumber) && !(await SealAlreadyExistsAsync(sealNumber)) && DateOnly.TryParse(dateString, out _);

        public async Task CreateSealAsync(string sealNumber, string dateString)
        {
            if (await CanCreateSealAsync(sealNumber, dateString))
            {
                _seal = await _repository.CreateSealAsync(sealNumber, DateOnly.Parse(dateString));
            }
            else
            {
                _seal = await _repository.GetSealAsync(sealNumber);
            }
        }

        private async Task<bool> SealAlreadyExistsAsync(string sealNumber) =>
            await _repository.GetSealAsync(sealNumber) != null;

        public List<string> GetOwnerInfo(int garageNumber)
        {
            Garage? garage = _repository.GetGarageAsync(garageNumber).Result;

            if (garage == null)
            {
                return new List<string>();
            }

            _owner = garage.Owner;

            return new List<string> { _owner.Name, _owner.Balance.ToString() };
        }

        public List<string> GetCounterInfo(int garageNumber)
        {
            Garage? garage = _repository.GetGarageAsync(garageNumber).Result;

            if (garage == null)
            {
                return new List<string>();
            }

            _counter = garage.Counter;

            return new List<string> { _counter.Number };
        }

        public List<string> GetSealInfo(int garageNumber)
        {
            Garage? garage = _repository.GetGarageAsync(garageNumber).Result;

            if (garage == null)
            {
                return new List<string>();
            }

            _seal = garage.Seal;

            return new List<string> { _seal.Number, _seal.Date.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture) };
        }

        public string SplitBlockOfGarage(Owner owner)
        {
            if (_garages.Count == 0 && owner != null)
            {
                return owner.Garages[0].Number.ToString();
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

            return result.ToString();
        }

        public async Task<string> SplitBlockOfGarageAsync(int garageNumber)
        {
            Garage? garage = await _repository.GetGarageAsync(garageNumber);

            if (garage == null)
            {
                return "Error 404";
            }

            if (_garages.Count == 0)
            {
                return garage.Number.ToString();
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

            return result.ToString();
        }

        public List<string> SplitAllBlockOfGarages()
        {
            List<Garage> garages = _repository.GetAllGarages();
            List<string> blocksOfGarages = new List<string>();

            foreach (Garage garage in garages)
            {
                blocksOfGarages.Add(SplitBlockOfGarage(garage.Owner));
            }

            return blocksOfGarages;
        }

        public void ParseBlockOfGarages(string blockOfGarages)
        {
            List<int> garageNumbers = new List<int>();

            foreach (string part in blockOfGarages.Split(','))
            {
                if (part.Contains('-'))
                {
                    string[] range = part.Split('-');
                    int start = int.Parse(range[0]);
                    int end = int.Parse(range[1]);

                    for (int i = start; i <= end; i++)
                    {
                        garageNumbers.Add(i);
                    }
                }
                else
                {
                    garageNumbers.Add(int.Parse(part));
                }
            }

            FillBlockOfGaragesAsync(garageNumbers);
        }

        public async Task FillBlockOfGaragesAsync(List<int> garageNumbers)
        {
            foreach (int garageNumber in garageNumbers)
            {
                Garage? garage = await _repository.GetGarageAsync(garageNumber);

                if (garage == null)
                {
                    garage = await _repository.CreateGarageAsync(garageNumber);
                }

                await SaveGarageAsync(garage);
                _garages.Add(garage);
            }
        }
    }
}
