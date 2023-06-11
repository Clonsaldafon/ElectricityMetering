using System.Windows;
using System.Windows.Controls;

namespace ElectricityMetering.WPF.Views.MessageLogs
{
    /// <summary>
    /// Interaction logic for SuccessfulSaveView.xaml
    /// </summary>
    public partial class SuccessfulSaveView : UserControl
    {
        private readonly ContentControl _messageLog;

        public SuccessfulSaveView(ContentControl messageLog)
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
