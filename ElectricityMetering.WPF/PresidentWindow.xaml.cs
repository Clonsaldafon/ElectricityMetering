using ElectricityMetering.Core.Controller;
using ElectricityMetering.Core.Model;
using System;
using System.Collections.Generic;
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
        private Payment _payment;
        private Tariff _tariff;

        public PresidentWindow()
        {
            InitializeComponent();
        }

        private void LoadInfoByGarageNumber(object sender, RoutedEventArgs e)
        {
            string garageNumber = TextBoxGarageNumber.Text;

            if (string.IsNullOrEmpty(garageNumber))
            {
                MessageBox.Show("Введите номер гаража!");
                return;
            }

            // TODO: garageNumber isn't always invalid
            if (_repository.CanLoadInfo(garageNumber))
            {
                _garage = _repository.LoadInfo(garageNumber);
                //_owner = _repository.LoadInfo(_garage);
                //_payment = _repository.LoadInfo(_owner);
                _tariff = _repository.LoadInfo();

                FillTextBoxes();
            }
            else
            {
                MessageBox.Show("Недопустимый номер гаража!");
            }
        }

        private void CreateNewGarageNumber(object sender, RoutedEventArgs e)
        {

        }

        private void SaveInfoByGarageNumber(object sender, RoutedEventArgs e)
        {

        }

        private void FillTextBoxes()
        {
            //TextBoxInPresidentWindowBlockOfGarages.Text = GetBlockOfGarages(_owner);
            TextBoxOwnerName.Text = _owner.FullName;
            TextBoxBalance.Text = _owner.Balance.ToString();

            TextBoxPaymentDate.Text = _payment.Date.ToString();
            TextBoxCash.Text = _payment.Cash.ToString();
            TextBoxNonCash.Text = _payment.NonCash.ToString();
            TextBoxPaymentTotal.Text = _payment.Total.ToString();

            TextBoxCounterNumber.Text = _garage.CounterNumber;
            TextBoxSealNumber.Text = _garage.SealNumber;
            TextBoxSealDate.Text = _garage.SealingDate.ToString();

            TextBoxTariffDate.Text = _tariff.Date.ToString();
            TextBoxTariff.Text = _tariff.Price.ToString();
            //TextBoxInPresidentWindowRemainder.Text = 
        }

        /*private string GetBlockOfGarages(Owner owner)
        {
            string[] garageNumbers = new string[owner.Garages.Count];

            for (int i = 0; i < owner.Garages.Count; i++)
            {
                garageNumbers[i] = owner.Garages[i].Number;
            }

            return string.Join(", ", garageNumbers);
        }*/
    }
}
