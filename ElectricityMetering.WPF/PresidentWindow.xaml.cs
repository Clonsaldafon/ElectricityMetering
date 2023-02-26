using ElectricityMetering.BL.Controller;
using ElectricityMetering.BL.Model;
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
using System.Windows.Shapes;

namespace ElectricityMetering.WPF
{
    /// <summary>
    /// Interaction logic for PresidentWindow.xaml
    /// </summary>
    public partial class PresidentWindow : Window
    {
        private Loader _loader = new Loader();

        private Garage _garage;
        private Owner _owner;
        private Payment _payment;
        private PricePerKw _pricePerKw;

        public PresidentWindow()
        {
            InitializeComponent();
        }

        private void LoadInfoByGarageNumber(object sender, RoutedEventArgs e)
        {
            string garageNumber = TextBoxGarageNumber.Text;

            if(!string.IsNullOrEmpty(TgarageNumber))
            {
                _loader.LoadInfo(_garage, _owner, _payment, _pricePerKw, garageNumber);
            }
            else
            {
                MessageBox.Show("Invalid GarageNumber!");
            }
        }
    }
}
