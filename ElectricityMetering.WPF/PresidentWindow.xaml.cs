using ElectricityMetering.WPF.Views;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ElectricityMetering.WPF
{
    /// <summary>
    /// Interaction logic for NewPresidentWindow.xaml
    /// </summary>
    public partial class PresidentWindow : Window
    {
        private readonly Dictionary<string, UserControl> _contentByRadioButtonName = new Dictionary<string, UserControl>()
        {
            { Properties.Resources.BalanceRadioButton, new BalanceView() },
            { Properties.Resources.TariffRadioButton, new TariffView() },
            { Properties.Resources.InfoRadioButton, new InfoView() },
            { Properties.Resources.PaymentRadioButton, new PaymentView() },
            { Properties.Resources.IndicationsRadioButton, new IndicationsView() }
        };

        public PresidentWindow()
        {
            InitializeComponent();

            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void ButtonHide_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void ButtonCloseApp_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void RadioButton_ShowContent(object sender, RoutedEventArgs e)
        {
            if (sender is not RadioButton)
            {
                return;
            }

            RadioButton radioButton = (RadioButton)sender;

            MainContent.Content = _contentByRadioButtonName[radioButton.Name];
        }
    }
}
