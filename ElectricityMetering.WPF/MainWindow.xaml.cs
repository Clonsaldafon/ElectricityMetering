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

        public void TestClick(object sender, RoutedEventArgs e)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                Garage testGarage1 = new Garage { Number = "1", SealNumber = "12345", SealingDate = DateOnly.Parse("18.02.2023"), CounterNumber = "54321" };
                Garage testGarage2 = new Garage { Number = "39", SealNumber = "6789", SealingDate = DateOnly.Parse("11.04.2020"), CounterNumber = "123789" };
                Owner testOwner = new Owner { FullName = "Anton", Balance = 0, Garages = new List<Garage> { testGarage1, testGarage2 } };

                db.Garages.AddRange(testGarage1, testGarage2);
                db.Owners.Add(testOwner);
                db.SaveChanges();
            }
        }
    }
}
