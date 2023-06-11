using ElectricityMetering.WPF.Views;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ElectricityMetering.WPF
{
    /// <summary>
    /// Interaction logic for ElectricianWindow.xaml
    /// </summary>
    public partial class ElectricianWindow : Window
    {
        private readonly Dictionary<string, UserControl> _contentByRadioButtonName = new Dictionary<string, UserControl>()
        {
            { Properties.Resources.TariffRadioButton, new TariffView() },
            { Properties.Resources.IndicationsRadioButton, new IndicationsView() }
        };

        public ElectricianWindow()
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
