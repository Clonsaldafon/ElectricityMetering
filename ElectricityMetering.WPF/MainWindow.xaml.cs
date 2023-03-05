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
    enum RoleName
    {
        President,
        Electrician
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void ApplicationInput(object sender, RoutedEventArgs e)
        {
            RoleName roleName = (RoleName)Enum.Parse(typeof(RoleName), RoleInput.Text);
            string password = PasswordInput.Password;

            if (!ApplicationInputHandler.PasswordIsCorrect(roleName.ToString(), password))
            {
                MessageBox.Show("Неверный пароль!");
                return;
            }

            if (roleName == RoleName.President)
            {
                PresidentWindow presidentWindow = new PresidentWindow();
                presidentWindow.Show();
                Close();
            }
            else if (roleName == RoleName.Electrician)
            {
                ElectricianWindow electricianWindow = new ElectricianWindow();
                electricianWindow.Show();
                Close();
            }
        }
    }
}
