﻿using ElectricityMetering.Core.Controllers;
using ElectricityMetering.WPF.Views.InfoViews;
using ElectricityMetering.WPF.Views.MessageLogs;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ElectricityMetering.WPF.Views
{
    /// <summary>
    /// Interaction logic for InfoView.xaml
    /// </summary>
    public partial class InfoView : UserControl
    {
        private readonly Controller _controller = new Controller();

        public InfoView()
        {
            InitializeComponent();
        }

        private async void LoadMainData(object sender, RoutedEventArgs e)
        {
            MessageLog.Content = new PleaseWaitTextView();
            Mouse.OverrideCursor = Cursors.Wait;

            string garageNumber = TextBoxGarageNumber.Text;

            ClearTextBoxes();

            if (int.TryParse(garageNumber, out int number))
            {
                if ((await _controller.LoadGarageAsync(number)) == null)
                {
                    MessageLog.Content = new NeedToCreateGarageView(number, MessageLog);
                    Mouse.OverrideCursor = null;
                    return;
                }
            }

            FillTextBoxesAsync();

            MessageLog.Content = new SuccessfulLoadView(MessageLog);
            Mouse.OverrideCursor = null;
        }

        private async void SaveMainData(object sender, RoutedEventArgs e)
        {
            MessageLog.Content = new PleaseWaitTextView();
            Mouse.OverrideCursor = Cursors.Wait;

            string garageNumber = TextBoxGarageNumber.Text;

            string ownerName = TextBoxOwnerName.Text;

            string counterNumber = TextBoxCounterNumber.Text;
            string sealNumber = TextBoxSealNumber.Text;
            string sealDateString = TextBoxSealDate.Text;
            string blockOfGarages = TextBoxBlockOfGarages.Text;

            await _controller.CreateOwnerAsync(ownerName);
            await _controller.CreateCounterAsync(counterNumber);
            await _controller.CreateSealAsync(sealNumber, sealDateString);

            await _controller.SaveGarageAsync(garageNumber);

            List<int> garageNumbers = _controller.ParseBlockOfGarages(blockOfGarages);
            await _controller.FillBlockOfGaragesAsync(garageNumbers);

            MessageLog.Content = new SuccessfulSaveView(MessageLog);
            Mouse.OverrideCursor = null;
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

        private async Task FillTextBoxesAsync()
        {
            if (int.TryParse(TextBoxGarageNumber.Text, out int garageNumber))
            {
                string blockOfGaragesInfo = await _controller.SplitBlockOfGarageAsync(garageNumber);
                List<string> ownerInfo = await _controller.GetOwnerInfoAsync(garageNumber);
                List<string> counterInfo = await _controller.GetCounterInfoAsync(garageNumber);
                List<string> sealInfo = await _controller.GetSealInfoAsync(garageNumber);

                TextBoxBlockOfGarages.Text = blockOfGaragesInfo;
                TextBoxOwnerName.Text = ownerInfo[0];
                TextBoxBalance.Text = ownerInfo[1];

                TextBoxCounterNumber.Text = counterInfo[0];

                TextBoxSealNumber.Text = sealInfo[0];
                TextBoxSealDate.Text = sealInfo[1];
            }
        }
    }
}
