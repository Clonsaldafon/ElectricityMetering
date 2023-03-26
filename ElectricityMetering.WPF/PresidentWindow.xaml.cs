using ElectricityMetering.Core;
using ElectricityMetering.Core.Controllers;
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
        private readonly Controller _controller = new Controller();

        private string _operationStatusText;

        public PresidentWindow()
        {
            InitializeComponent();
        }

        private async void AddMainData(object sender, RoutedEventArgs e)
        {
            string garageNumber = TextBoxGarageNumber.Text;

            _operationStatusText = await _controller.AddGarageAsync(garageNumber);
            MessageBox.Show(_operationStatusText);

            ClearTextBoxes();
            _operationStatusText = FillTextBoxes();
            MessageBox.Show(_operationStatusText);
        }

        private async void LoadMainData(object sender, RoutedEventArgs e)
        {
            string garageNumber = TextBoxGarageNumber.Text;

            _operationStatusText = await _controller.LoadGarageAsync(garageNumber);
            MessageBox.Show(_operationStatusText);

            ClearTextBoxes();
            _operationStatusText = FillTextBoxes();
            MessageBox.Show(_operationStatusText);
        }

        private async void SaveMainData(object sender, RoutedEventArgs e)
        {
            string ownerName = TextBoxOwnerName.Text;
            string balanceString = TextBoxBalance.Text;

            string counterNumber = TextBoxCounterNumber.Text;
            string sealNumber = TextBoxSealNumber.Text;
            string sealDateString = TextBoxSealDate.Text;

            string[] garageNumbers = TextBoxBlockOfGarages.Text.Split(",");

            _operationStatusText = await _controller.AddOwnerAsync(ownerName, balanceString);
            MessageBox.Show(_operationStatusText);

            _operationStatusText = await _controller.AddCounterAsync(counterNumber);
            MessageBox.Show(_operationStatusText);

            _operationStatusText = await _controller.AddSealAsync(sealNumber, sealDateString);
            MessageBox.Show(_operationStatusText);

            foreach (string number in garageNumbers)
            {
                _operationStatusText = await _controller.AddGarageAsync(number);
                MessageBox.Show(_operationStatusText);
            }

            MessageBox.Show("Данные успешно сохранены.");
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

        private string FillTextBoxes()
        {
            string blockOfGaragesInfo = _controller.SplitBlockOfGarage();
            List<string> ownerInfo = _controller.GetOwnerInfo();
            List<string> counterInfo = _controller.GetCounterInfo();
            List<string> sealInfo = _controller.GetSealInfo();

            TextBoxBlockOfGarages.Text = blockOfGaragesInfo;
            TextBoxOwnerName.Text = ownerInfo[0];
            TextBoxBalance.Text = ownerInfo[1];

            TextBoxCounterNumber.Text = counterInfo[0];

            TextBoxSealNumber.Text = sealInfo[0];
            TextBoxSealDate.Text = sealInfo[1];

            return "Данные успешно загружены.";
        }
    }
}
