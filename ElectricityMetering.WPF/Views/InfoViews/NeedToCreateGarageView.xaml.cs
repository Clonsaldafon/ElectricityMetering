using ElectricityMetering.Core.Controllers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ElectricityMetering.WPF.Views.InfoViews
{
    /// <summary>
    /// Interaction logic for NeedToCreateGarageView.xaml
    /// </summary>
    public partial class NeedToCreateGarageView : UserControl
    {
        private readonly Controller _controller = new Controller();

        private readonly InfoView _infoView = new InfoView();
        private readonly ContentControl _messageLog;

        private int _garageNumber;

        public NeedToCreateGarageView(int garageNumber, ContentControl messageLog)
        {
            InitializeComponent();
            _messageLog = messageLog;

            _garageNumber = garageNumber;

            TextBlockGarageNumberInError.Text = _garageNumber.ToString();
        }

        private void CreateGarage(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;

            _ = _controller.CreateGarageAsync(_garageNumber);

            _messageLog.Content = new GarageCreatedView(_messageLog);

            Mouse.OverrideCursor = null;
        }

        private void CloseMessage(object sender, RoutedEventArgs e)
        {
            _messageLog.Content = null;
        }
    }
}
