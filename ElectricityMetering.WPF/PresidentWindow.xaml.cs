using ElectricityMetering.Core;
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
    /// Interaction logic for PresidentWindow.xaml
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
                MessageBox.Show("GarageNumber is null!");
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
                MessageBox.Show("Invalid GarageNumber!");
            }
        }

        private void FillTextBoxes()
        {
            TextBoxInPresidentWindowGarageNumber.Text = _garage.Number;
            //TextBoxInPresidentWindowBlockOfGarages.Text = GetBlockOfGarages(_owner);
            TextBoxInPresidentWindowOwnerFullName.Text = _owner.FullName;
            TextBoxInPresidentWindowBalance.Text = _owner.Balance.ToString();

            TextBoxInPresidentWindowPaymentDate.Text = _payment.Date.ToString();
            TextBoxInPresidentWindowCash.Text = _payment.Cash.ToString();
            TextBoxInPresidentWindowNonCash.Text = _payment.NonCash.ToString();
            TextBoxInPresidentWindowPaymentTotal.Text = _payment.Total.ToString();

            TextBoxInPresidentWindowRemainderDate.Text = _tariff.Date.ToString();
            TextBoxInPresidentWindowPricePerKw.Text = _tariff.Price.ToString();
            //TextBoxInPresidentWindowRemainder.Text = 

            TextBoxInPresidentWindowCounterNumber.Text = _garage.CounterNumber;
            TextBoxInPresidentWindowSealNumber.Text = _garage.SealNumber;
            TextBoxInPresidentWindowSealDate.Text = _garage.SealingDate.ToString();
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
