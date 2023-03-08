using ElectricityMetering.Core.Controller;
using ElectricityMetering.Core.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ElectricityMetering.WPF
{
    /// <summary>
    /// Interaction logic for NewPresidentWindow.xaml
    /// </summary>
    public partial class PresidentWindow : Window
    {
        private Repository _repository = new Repository();

        private Garage _garage;
        private Owner _owner;
        /*private Payment _payment;
        private Tariff _tariff;*/

        public PresidentWindow()
        {
            InitializeComponent();
        }

        private void Load(object sender, RoutedEventArgs e)
        {
            string garageNumber = TextBoxGarageNumber.Text;

            if (string.IsNullOrEmpty(garageNumber))
            {
                MessageBox.Show("Введите номер гаража!");
                return;
            }

            if (!_repository.CanLoadGarage(garageNumber))
            {
                MessageBox.Show("Таких данных нет!");
                return;
            }

            _garage = _repository.LoadGarage(garageNumber);

            if (_repository.CanLoadOwner(garageNumber))
            {
                _owner = _repository.LoadOwner(_garage);
            }

            //_payment = _repository.LoadInfo(_owner);
            //_tariff = _repository.LoadInfo();

            // TODO: _garage.Owner is null
            //MessageBox.Show(_garage.Owner.Name);
            FillTextBoxes();
        }

        private void Add(object sender, RoutedEventArgs e)
        {
            string garageNumber = TextBoxGarageNumber.Text;

            if (!_repository.CanCreateNewGarage(garageNumber))
            {
                MessageBox.Show("Такой гараж уже существует!");
                return;
            }

            if (string.IsNullOrEmpty(garageNumber))
            {
                MessageBox.Show("Введите номер гаража!");
                return;
            }

            _repository.CreateNewGarage(garageNumber);
            MessageBox.Show("Гараж добавлен.");
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            string ownerName = TextBoxOwnerName.Text;
            decimal balance = decimal.Parse(TextBoxBalance.Text, CultureInfo.InvariantCulture);

            string counterNumber = TextBoxCounterNumber.Text;
            string sealNumber = TextBoxSealNumber.Text;
            DateOnly sealDate = DateOnly.Parse(TextBoxSealDate.Text);

            _garage.CounterNumber = counterNumber;
            _garage.SealNumber = sealNumber;
            _garage.SealDate = sealDate;
            _repository.SaveGarage(_garage);

            if (!string.IsNullOrEmpty(ownerName) && _repository.CanCreateNewOwner(_garage))
            {
                _repository.CreateNewOwner(ownerName, balance, _garage.Number);
                MessageBox.Show("Владелец добавлен.");
            }

            _owner = _repository.LoadOwner(_garage);

            string[] garageNumbers = TextBoxBlockOfGarages.Text.Split(",");

            foreach (string garageNumber in garageNumbers)
            {
                if (_repository.CanCreateNewGarage(garageNumber))
                {
                    _repository.CreateNewGarage(garageNumber, sealNumber, counterNumber, sealDate);
                    _owner.Garages.Append(_repository.LoadGarage(garageNumber));
                }
            }
        }

        private void FillTextBoxes()
        {
            if (_owner is not null)
            {
                TextBoxBlockOfGarages.Text = GetBlockOfGarages(_owner);
                TextBoxOwnerName.Text = _owner.Name;
                TextBoxBalance.Text = _owner.Balance.ToString();
            }

            //TextBoxPaymentDate.Text = _payment.Date.ToString();
            //TextBoxCash.Text = _payment.Cash.ToString();
            //TextBoxNonCash.Text = _payment.NonCash.ToString();
            //TextBoxPaymentTotal.Text = _payment.Total.ToString();

            TextBoxCounterNumber.Text = _garage.CounterNumber;
            TextBoxSealNumber.Text = _garage.SealNumber;
            TextBoxSealDate.Text = _garage.SealDate.ToString();

            //TextBoxTariffDate.Text = _tariff.Date.ToString();
            //TextBoxTariff.Text = _tariff.Price.ToString();
            //TextBoxInPresidentWindowRemainder.Text = 
        }

        private string GetBlockOfGarages(Owner owner)
        {
            string[] garageNumbers = new string[owner.Garages.Count];

            for (int i = 0; i < owner.Garages.Count; i++)
            {
                garageNumbers[i] = owner.Garages[i].Number;
            }

            return string.Join(", ", garageNumbers);
        }
    }
}
