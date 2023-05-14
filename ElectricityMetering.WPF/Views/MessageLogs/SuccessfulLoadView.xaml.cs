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

namespace ElectricityMetering.WPF.Views.MessageLogs
{
    /// <summary>
    /// Логика взаимодействия для SuccessfulLoadView.xaml
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
