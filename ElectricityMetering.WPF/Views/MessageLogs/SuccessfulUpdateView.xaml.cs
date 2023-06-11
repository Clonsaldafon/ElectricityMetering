using System.Windows;
using System.Windows.Controls;

namespace ElectricityMetering.WPF.Views.MessageLogs
{
    /// <summary>
    /// Interaction logic for SuccessfulUpdateView.xaml
    /// </summary>
    public partial class SuccessfulUpdateView : UserControl
    {
        private readonly ContentControl _messageLog;

        public SuccessfulUpdateView(ContentControl messageLog)
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
