using System.Windows;
using System.Windows.Controls;

namespace ElectricityMetering.WPF.Views.MessageLogs
{
    /// <summary>
    /// Interaction logic for SuccessfulLoadView.xaml
    /// </summary>
    public partial class SuccessfulLoadView : UserControl
    {
        private readonly ContentControl _messageLog;

        public SuccessfulLoadView(ContentControl messageLog)
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
