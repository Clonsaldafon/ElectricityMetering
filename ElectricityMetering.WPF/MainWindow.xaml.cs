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

            // Add test data in DB
            /*using (ApplicationContext db = new ApplicationContext())
            {
                Role president = new Role { Name = "President", Password = "0000" };
                Role electrician = new Role { Name = "Electrician", Password = "qwerty" };
                db.AddRange(president, electrician);

                Indication i1 = new Indication { Indications = new int[36] { 475, 475, 475, 476, 476, 476, 476, 481, 481, 481, 481, 481, 481, 481, 481, 481, 487, 487, 487, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 } };
                Indication i2 = new Indication { Indications = new int[36] { 366, 366, 366, 377, 377, 377, 393, 393, 393, 393, 393, 400, 400, 400, 400, 404, 404, 404, 415, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 } };
                Indication i3 = new Indication { Indications = new int[36] { 159, 159, 159, 159, 159, 159, 159, 159, 159, 159, 159, 159, 159, 159, 159, 159, 159, 159, 159, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 } };
                Indication i89 = new Indication { Indications = new int[36] { 1666, 1666, 1666, 1698, 1698, 1698, 1764, 1764, 1764, 1764, 1764, 1843, 1843, 1843, 1843, 1949, 1949, 1949, 1988, 1999, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 } };
                Indication i90 = new Indication { Indications = new int[36] { 1666, 1666, 1666, 1698, 1698, 1698, 1764, 1764, 1764, 1764, 1764, 1843, 1843, 1843, 1843, 1949, 1949, 1949, 1988, 1999, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 } };
                Indication i111 = new Indication { Indications = new int[36] { 519, 519, 519, 519, 519, 519, 521, 521, 521, 521, 521, 522, 522, 522, 522, 522, 522, 522, 532, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 } };
                db.Indications.AddRange(i1, i2, i3, i89, i90, i111);

                Garage g1 = new Garage { Number = "1", CounterNumber = "596443", SealNumber = "14812939-41", SealingDate = DateOnly.Parse("15.04.2019"), Indication = i1 };
                Garage g2 = new Garage { Number = "2", CounterNumber = "129304", SealNumber = "14812959-60", SealingDate = DateOnly.Parse("24.04.2019"), Indication = i2 };
                Garage g3 = new Garage { Number = "3", CounterNumber = "ОО7791047013237", SealNumber = "14812039;40", SealingDate = DateOnly.Parse("18.05.2019"), Indication = i3 };
                Garage g89 = new Garage { Number = "89", CounterNumber = "О108016679", SealNumber = "14812001-03", SealingDate = DateOnly.Parse("07.05.2019"), Indication = i89 };
                Garage g90 = new Garage { Number = "90", CounterNumber = "О108016679", SealNumber = "14812001-03", SealingDate = DateOnly.Parse("07.05.2019"), Indication = i90 };
                Garage g111 = new Garage { Number = "111", CounterNumber = "О36978", SealNumber = "14812772,73", SealingDate = DateOnly.Parse("14.03.2017"), Indication = i111 };
                db.Garages.AddRange(g1, g2, g3, g89, g90, g111);

                Owner o1 = new Owner { FullName = "Петухова Н.И.", Garages = new List<Garage> { g1 }, Balance = 249.29 };
                Owner o2 = new Owner { FullName = "Торбиев С.А.", Garages = new List<Garage> { g2 }, Balance = 198.49 };
                Owner o3 = new Owner { FullName = "Меркушев А.В.", Garages = new List<Garage> { g3 }, Balance = 322.50 };
                Owner o8990 = new Owner { FullName = "Мосягин С.А.", Garages = new List<Garage> { g89, g90 }, Balance = 466.09 };
                Owner o111 = new Owner { FullName = "Мухамадеева Н.В.", Garages = new List<Garage> { g111 }, Balance = 1197.06 };
                db.Owners.AddRange(o1, o2, o3, o8990, o111);

                List<Payment> payments = new List<Payment>
                {
                    new Payment {  Date = DateOnly.Parse("05.06.2021"), Cash = 120, NonCash = 0, Total = 120, Owner = o3 },
                    new Payment {  Date = DateOnly.Parse("05.06.2021"), Cash = 120, NonCash = 0, Total = 120, Owner = o3 },
                    new Payment {  Date = DateOnly.Parse("17.07.2021"), Cash = 1400, NonCash = 0, Total = 1400, Owner = o8990 },
                    new Payment {  Date = DateOnly.Parse("23.04.2022"), Cash = 200, NonCash = 0, Total = 200, Owner = o2 },
                    new Payment {  Date = DateOnly.Parse("04.06.2022"), Cash = 50, NonCash = 0, Total = 50, Owner = o2 }
                };
                db.Payments.AddRange(payments);

                BalanceRate balanceRate = new BalanceRate { Debt = -8836.87, Advance = 119506.49, Balance = 110669.62 };
                db.BalanceRates.Add(balanceRate);

                List<PricePerKw> pricesPerKw = new List<PricePerKw>
                {
                    new PricePerKw { Date = DateOnly.Parse("01.01.2021"), Price = 3.71 },
                    new PricePerKw { Date = DateOnly.Parse("01.07.2021"), Price = 3.83 },
                    new PricePerKw { Date = DateOnly.Parse("01.01.2022"), Price = 3.83 },
                    new PricePerKw { Date = DateOnly.Parse("01.07.2022"), Price = 3.95 }
                };
                db.PricesPerKw.AddRange(pricesPerKw);

                db.SaveChanges();
            }*/
        }

        public void ApplicationInput(object sender, RoutedEventArgs e)
        {
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
            else
            {
                MessageBox.Show("Invalid password!");
            }
        }
    }
}
