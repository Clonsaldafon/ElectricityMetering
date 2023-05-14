using ElectricityMetering.Core.Controllers;
using ElectricityMetering.Core.Models;
using ElectricityMetering.WPF.Views.InfoViews;
using ElectricityMetering.WPF.Views.MessageLogs;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

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

            FillTextBoxes();

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

            await _controller.SaveGarageAsync(int.Parse(garageNumber));

            _controller.ParseBlockOfGarages(blockOfGarages);

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

        private void FillTextBoxes()
        {
            int garageNumber = int.Parse(TextBoxGarageNumber.Text);

            string blockOfGaragesInfo = _controller.SplitBlockOfGarageAsync(garageNumber).Result;
            List<string> ownerInfo = _controller.GetOwnerInfo(garageNumber);
            List<string> counterInfo = _controller.GetCounterInfo(garageNumber);
            List<string> sealInfo = _controller.GetSealInfo(garageNumber);

            TextBoxBlockOfGarages.Text = blockOfGaragesInfo;
            TextBoxOwnerName.Text = ownerInfo[0];
            TextBoxBalance.Text = ownerInfo[1];

            TextBoxCounterNumber.Text = counterInfo[0];

            TextBoxSealNumber.Text = sealInfo[0];
            TextBoxSealDate.Text = sealInfo[1];
        }
    }
}
