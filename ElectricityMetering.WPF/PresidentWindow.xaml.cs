using ElectricityMetering.Core;
using ElectricityMetering.Core.Controllers;
using ElectricityMetering.WPF.Views;
using ElectricityMetering.WPF.Views.InfoViews;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interop;

namespace ElectricityMetering.WPF
{
    /// <summary>
    /// Interaction logic for NewPresidentWindow.xaml
    /// </summary>
    public partial class PresidentWindow : Window
    {
        private readonly SignInController _signInController = new SignInController();

        public PresidentWindow()
        {
            InitializeComponent();

            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
        }

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);

        private void ControlBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            /*WindowInteropHelper helper = new WindowInteropHelper(this);
            SendMessage(helper.Handle, 161, 2, 0);*/
            DragMove();
        }

        private void ButtonHide_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private async void ButtonCloseAsync_Click(object sender, RoutedEventArgs e)
        {
            await _signInController.ExitAsync("Председатель");

            Application.Current.Shutdown();
        }

        private void RadioButton_ShowContent(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;

            switch (radioButton.Name)
            {
                case "BalanceRadioButton":
                    MainContent.Content = new BalanceView();
                    break;
                case "TariffRadioButton":
                    MainContent.Content = new TariffView();
                    break;
                case "InfoRadioButton":
                    MainContent.Content = new InfoView();
                    break;
                case "PaymentRadioButton":
                    MainContent.Content = new PaymentView();
                    break;
                case "IndicationsRadioButton":
                    MainContent.Content = new IndicationsView();
                    break;
            }
        }
    }
}
