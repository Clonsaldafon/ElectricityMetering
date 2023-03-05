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

        public MainWindow()
        {
            InitializeComponent();

            /*using(ApplicationContext db = new ApplicationContext())
            {
                Role role1 = new Role { Name = _president, Password = "0000" };
                Role role2 = new Role { Name = _electrician, Password = "qwerty" };

                db.Roles.AddRange(role1, role2);
                db.SaveChanges();
            }*/
        }

        public void ApplicationInput(object sender, RoutedEventArgs e)
        {
            string roleName = RoleInput.Text;
            string password = PasswordInput.Password;

            if (!ApplicationInputHandler.PasswordIsCorrect(roleName.ToString(), password))
            {
                MessageBox.Show("Неверный пароль!");
                return;
            }

            if (string.Equals(roleName, _president))
            {
                PresidentWindow presidentWindow = new PresidentWindow();
                presidentWindow.Show();
                Close();
            }
            else if (string.Equals(roleName, _electrician))
            {
                ElectricianWindow electricianWindow = new ElectricianWindow();
                electricianWindow.Show();
                Close();
            }
        }
    }
}
