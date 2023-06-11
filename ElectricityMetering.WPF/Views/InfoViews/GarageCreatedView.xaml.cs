using System.Windows;
using System.Windows.Controls;

namespace ElectricityMetering.WPF.Views.InfoViews
{
    /// <summary>
    /// Interaction logic for GarageCreatedView.xaml
    /// </summary>
    public partial class GarageCreatedView : UserControl
    {
        private readonly InfoView _infoView = new InfoView();
        private readonly ContentControl _messageLog;

        public GarageCreatedView(ContentControl messageLog)
        {
            InitializeComponent();
            _messageLog = messageLog;
        }

        private void CloseMessage(object sender, RoutedEventArgs e)
        {
            _messageLog.Content = null;
        }
    }
}
