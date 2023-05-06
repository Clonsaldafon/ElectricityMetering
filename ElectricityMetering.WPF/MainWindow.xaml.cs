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
using ElectricityMetering.Core;
using ElectricityMetering.Core.Models;
using ElectricityMetering.Core.Controllers;
using Accessibility;
using System.Data;
using System.Threading;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Globalization;

namespace ElectricityMetering.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SignInController _signInController = new SignInController();

        private Dictionary<string, Window> _windowsByRoleName = new Dictionary<string, Window>
        {
            { "Председатель", new PresidentWindow()},
            { "Электрик", new ElectricianWindow()},
        };

        private readonly SolidColorBrush _successColor = new SolidColorBrush(Color.FromRgb(74, 225, 127));
        private readonly SolidColorBrush _failureColor = new SolidColorBrush(Color.FromRgb(214, 64, 64));

        public MainWindow()
        {
            InitializeComponent();
        }

        public async void ButtonSignIn_Click(object sender, RoutedEventArgs e)
        {
            string roleName = RoleInput.Text;
            string password = PasswordInput.Password;

            if (!(await _signInController.PasswordIsCorrectAsync(roleName, password)))
            {
                PasswordInput.Clear();
                
                PasswordInput.BorderBrush = _failureColor;
                TextBlockPassword.Text = "Неверный пароль";
                TextBlockPassword.Foreground = _failureColor;

                return;
            }

            /*if (await _signInController.RoleIsActiveAsync(roleName))
            {
                RoleInput.SelectedIndex = -1;
                PasswordInput.Clear();

                RoleInput.BorderBrush = _failureColor;
                TextBlockRole.Text = "Уже вошли";
                TextBlockRole.Foreground = _failureColor;

                return;
            }*/

            PasswordInput.BorderBrush = _successColor;
            TextBlockPassword.Text = "Пароль верный";
            TextBlockPassword.Foreground = _successColor;
            RoleInput.BorderBrush = _successColor;
            TextBlockRole.Foreground = _successColor;

            await _signInController.SignInAsync(roleName);

            _windowsByRoleName[roleName].Show();
            Close();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
             DragMove();
        }

        private void ButtonHide_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
