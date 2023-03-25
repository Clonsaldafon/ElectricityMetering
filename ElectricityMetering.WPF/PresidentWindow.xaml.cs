using ElectricityMetering.Core.Controller;
using ElectricityMetering.Core.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;

namespace ElectricityMetering.WPF
{
    /// <summary>
    /// Interaction logic for NewPresidentWindow.xaml
    /// </summary>
    public partial class PresidentWindow : Window
    {
        private Repository _repository = new Repository();

        private Garage? _garage;
        private Owner? _owner;
        private Counter? _counter;
        private Seal? _seal;

        private Payment? _payment;

        public PresidentWindow()
        {
            InitializeComponent();
        }

        private async void AddAsync(object sender, RoutedEventArgs e)
        {
            string garageNumber = TextBoxGarageNumber.Text;

            if (string.IsNullOrEmpty(garageNumber))
            {
                MessageBox.Show("Введите номер гаража!");
                return;
            }

            if ((await _repository.GetGarageAsync(garageNumber)) != null)
            {
                MessageBox.Show("Такой гараж уже существует!");
                return;
            }

            _garage = await _repository.CreateGarageAsync(garageNumber);
            MessageBox.Show($"Гараж №{_garage.Number} добавлен.");

            _owner = _garage.Owner;
            _counter = _garage.Counter;
            _seal = _garage.Seal;

            ClearTextBoxes();
            FillTextBoxes();
        }

        private async void LoadAsync(object sender, RoutedEventArgs e)
        {
            string garageNumber = TextBoxGarageNumber.Text;

            if (string.IsNullOrEmpty(garageNumber))
            {
                MessageBox.Show("Введите номер гаража!");
                return;
            }

            _garage = await _repository.GetGarageAsync(garageNumber);
            if (_garage == null)
            {
                MessageBox.Show("Таких данных нет!");
                return;
            }

            _owner = _garage.Owner;
            _counter = _garage.Counter;
            _seal = _garage.Seal;

            ClearTextBoxes();
            FillTextBoxes();
            MessageBox.Show("Данные загружены.");
        }

        private async void SaveAsync(object sender, RoutedEventArgs e)
        {
            string garageNumber = TextBoxGarageNumber.Text;
            string ownerName = TextBoxOwnerName.Text;
            decimal balance = decimal.Parse(TextBoxBalance.Text, CultureInfo.InvariantCulture);

            string counterNumber = TextBoxCounterNumber.Text;
            string sealNumber = TextBoxSealNumber.Text;
            DateOnly sealDate = DateOnly.Parse(TextBoxSealDate.Text);

            // TODO: save _payment data

            _owner = await _repository.GetOwnerAsync(_garage);
            if (_owner.Name == "-")
            {
                _owner = await _repository.CreateOwnerAsync(ownerName, balance);
                MessageBox.Show($"Владелец {_owner.Name} добавлен.");
            }

            _counter = await _repository.GetCounterAsync(_garage);
            if (_counter.Number == "-")
            {
                _counter = await _repository.CreateCounterAsync(counterNumber);
                MessageBox.Show($"Счетчик №{_counter.Number} добавлен.");
            }

            _seal = await _repository.GetSealAsync(_garage);
            if (_seal.Number == "-")
            {
                _seal = await _repository.CreateSealAsync(sealNumber, sealDate);
                MessageBox.Show($"Пломба №{_seal.Number} добавлена.");
            }

            string[] garageNumbers = TextBoxBlockOfGarages.Text.Split(",");
            foreach (string number in garageNumbers)
            {
                if (await _repository.GetGarageAsync(number) == null && !string.IsNullOrEmpty(number))
                {
                    Garage garage = await _repository.CreateGarageAsync(number);
                    await _repository.SaveGarageAsync(garage, _owner, _counter, _seal);

                    garage = await _repository.GetGarageAsync(number);
                    MessageBox.Show($"Гараж №{garage.Number} добавлен.");
                }
            }

            await _repository.SaveGarageAsync(_garage, _owner, _counter, _seal);
            _garage = await _repository.GetGarageAsync(_owner);

            MessageBox.Show("Данные сохранены.");
        }

        private void FillTextBoxes()
        {
            if (_owner != null)
            {
                TextBoxBlockOfGarages.Text = SplitBlockOfGarages(_repository.GetBlockOfGarages(_garage));
                TextBoxOwnerName.Text = _owner.Name;
                TextBoxBalance.Text = _owner.Balance.ToString();
            }

            if (_counter != null)
            {
                TextBoxCounterNumber.Text = _counter.Number;
            }
            
            if (_seal != null)
            {
                TextBoxSealNumber.Text = _seal.Number;
                TextBoxSealDate.Text = _seal.Date.ToString();
            }

            // TODO: fill _payment data
        }

        private void ClearTextBoxes()
        {
            TextBoxBlockOfGarages.Clear();
            TextBoxOwnerName.Clear();
            TextBoxBalance.Clear();

            TextBoxCounterNumber.Clear();
            TextBoxSealNumber.Clear();
            TextBoxSealDate.Clear();
        }

        private string SplitBlockOfGarages(List<Garage> garages)
        {
            StringBuilder result = new StringBuilder();

            foreach (Garage garage in garages)
            {
                result.Append($"{garage.Number},");
            }

            return result.ToString().Remove(result.Length - 1);
        }
    }
}
