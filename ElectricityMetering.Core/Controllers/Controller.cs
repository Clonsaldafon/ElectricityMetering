using ElectricityMetering.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricityMetering.Core.Controllers
{
    public class Controller
    {
        private readonly Repository _repository = new Repository();

        private Garage? _garage;
        private Owner? _owner;
        private Counter? _counter;
        private Seal? _seal;
        private Payment? _payment;

        private List<Garage> _garages;

        public async Task<string> AddGarageAsync(string garageNumber)
        {
            if (string.IsNullOrEmpty(garageNumber))
            {
                return $"Поле для ввода номера гаража не может быть пустым!";
            }

            if (await GarageAlreadyExistsAsync(garageNumber))
            {
                _garage = await _repository.GetGarageAsync(garageNumber);

                await FillDataByGarageAsync(_garage);

                return "Гараж с таким номером уже содержится в базе данных!";
            }

            _garage = await _repository.CreateGarageAsync(garageNumber);
            await _repository.SaveGarageAsync(_garage, _owner, _counter, _seal);
            _garage = await _repository.GetGarageAsync(garageNumber);

            await FillDataByGarageAsync(_garage);

            return $"Гараж №{_garage.Number} успешно добавлен.";
        }

        public async Task<string> LoadGarageAsync(string garageNumber)
        {
            if (string.IsNullOrEmpty(garageNumber))
            {
                return $"Поле для ввода номера гаража не может быть пустым!";
            }

            if (!(await GarageAlreadyExistsAsync(garageNumber)))
            {
                return "В базе данных нет гаража с таким номером!";
            }

            _garage = await _repository.GetGarageAsync(garageNumber);
            FillDataByGarageAsync(_garage);

            return $"Данные по гаражу №{_garage.Number} успешно получены.";
        }

        private async Task<bool> GarageAlreadyExistsAsync(string garageNumber)
        {
            return (await _repository.GetGarageAsync(garageNumber)) != null;
        }

        private async Task FillDataByGarageAsync(Garage garage)
        {
            _owner = await _repository.GetOwnerAsync(garage);
            _counter = await _repository.GetCounterAsync(garage);
            _seal = await _repository.GetSealAsync(garage);
            _garages = _repository.GetBlockOfGarages(garage);
        }

        public async Task<string> AddOwnerAsync(string ownerName, string balanceString)
        {
            if (ownerName != "-")
            {
                return $"Гараж №{_garage.Number} пока что без владельца.";
            }

            if (string.IsNullOrEmpty(ownerName))
            {
                return "Поле для ввода ФИО владельца не может быть пустым!";
            }

            if (await OwnerAlreadyExistsAsync(ownerName))
            {
                return $"У гаража №{_garage.Number} владелец не изменился.";
            }

            if (string.IsNullOrEmpty(balanceString))
            {
                return $"Поле для ввода баланса не может быть пустым!"; ;
            }

            if (decimal.TryParse(balanceString, out decimal balance))
            {
                _owner = await _repository.CreateOwnerAsync(ownerName, balance);

                return $"Теперь у гаража №{_garage.Number} новый владелец: {_owner.Name}";
            }
            else
            {
                return $"Некорректный формат данных: {balanceString}";
            }
        }

        private async Task<bool> OwnerAlreadyExistsAsync(string ownerName)
        {
            return await _repository.GetOwnerAsync(ownerName) != null;
        }

        public async Task<string> AddCounterAsync(string counterNumber)
        {
            if (string.IsNullOrEmpty(counterNumber))
            {
                return "Поле для ввода номера счетчика не может быть пустым!";
            }

            if (await CounterAlreadyExistsAsync(counterNumber))
            {
                return $"У гаража №{_garage.Number} счетчик не изменился.";
            }

            _counter = await _repository.CreateCounterAsync(counterNumber);

            return $"Теперь у гаража №{_garage.Number} новый счетчик: {_counter.Number}";
        }

        private async Task<bool> CounterAlreadyExistsAsync(string counterNumber)
        {
            return await _repository.GetCounterAsync(counterNumber) != null;
        }

        public async Task<string> AddSealAsync(string sealNumber, string sealDateString)
        {
            if (string.IsNullOrEmpty(sealNumber))
            {
                return $"Поле для ввода номера пломбы не может быть пустым!";
            }

            if (await SealAlreadyExistsAsync(sealNumber))
            {
                return $"У гаража №{_garage} пломба не изменилась.";
            }

            if (string.IsNullOrEmpty(sealDateString))
            {
                return $"Поле для ввода даты опломбирования не может быть пустым!";
            }

            if (DateOnly.TryParse(sealDateString, out DateOnly sealDate))
            {
                _seal = await _repository.CreateSealAsync(sealNumber, sealDate);

                return $"Теперь у гаража №{_garage.Number} новая пломба: {_seal.Number}";
            }
            else
            {
                return $"Некорректный формат данных: {sealDateString}"; ;
            }
        }

        private async Task<bool> SealAlreadyExistsAsync(string sealNumber)
        {
            return await _repository.GetSealAsync(sealNumber) != null;
        }

        public List<string> GetOwnerInfo()
        {
            return new List<string> { _owner.Name, _owner.Balance.ToString() };
        }

        public List<string> GetCounterInfo()
        {
            return new List<string> { _counter.Number };
        }

        public List<string> GetSealInfo()
        {
            return new List<string> { _seal.Number, _seal.Date.ToString() };
        }

        public string SplitBlockOfGarage()
        {
            StringBuilder result = new StringBuilder();

            foreach (Garage garage in _garages)
            {
                result.Append($"{garage.Number},");
            }

            return result.ToString().Remove(result.Length - 1);
        }
    }
}
