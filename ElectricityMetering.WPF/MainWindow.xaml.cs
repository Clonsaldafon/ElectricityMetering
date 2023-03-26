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

        private string _operationStatusText;

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
                _operationStatusText = "Неверный пароль!";
                MessageBox.Show(_operationStatusText);
                return;
            }

            _operationStatusText = "Вы успешно вошли в систему!";
            MessageBox.Show(_operationStatusText);

            _windowsByRoleName[roleName].Show();
            Close();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void ButtonMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
