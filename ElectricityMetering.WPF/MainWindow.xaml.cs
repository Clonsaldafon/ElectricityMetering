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
using ElectricityMetering.Core.Model;
using ElectricityMetering.Core.Controller;

namespace ElectricityMetering.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly string _president = "Председатель";
        private readonly string _electrician = "Электрик";

        private ApplicationInputHandler _applicationInputHandler = new ApplicationInputHandler();

        public MainWindow()
        {
            InitializeComponent();
        }

        public async void ApplicationInputAsync(object sender, RoutedEventArgs e)
        {
            string roleName = RoleInput.Text;
            string password = PasswordInput.Password;

            if (!(await _applicationInputHandler.PasswordIsCorrectAsync(roleName.ToString(), password)))
            {
                MessageBox.Show("Неверный пароль!");
                return;
            }

            if (roleName == _president)
            {
                PresidentWindow presidentWindow = new PresidentWindow();
                presidentWindow.Show();
                Close();
            }
            else if (roleName == _electrician)
            {
                ElectricianWindow electricianWindow = new ElectricianWindow();
                electricianWindow.Show();
                Close();
            }
        }
    }
}
