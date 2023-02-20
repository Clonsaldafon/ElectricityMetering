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
using ElectricityMetering.BL;
using ElectricityMetering.BL.Model;
using ElectricityMetering.BL.Controller;

namespace ElectricityMetering.WPF
{
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
            using (ApplicationContext db = new ApplicationContext())
            {
                Role role = new Role { Name = "President", Password = "0000"};
            }

            string roleName = RoleInput.Text;
            string password = PasswordInput.Password;

            if (ApplicationInputHandler.PasswordIsCorrect(roleName, password))
            {
                if (roleName == "President")
                {
                    PresidentWindow presidentWindow = new PresidentWindow();
                    presidentWindow.Show();
                    Close();
                }
                else if (roleName == "Electrician")
                {
                    ElectricianWindow electricianWindow = new ElectricianWindow();
                    electricianWindow.Show();
                    Close();
                }
            }
        }
    }
}
