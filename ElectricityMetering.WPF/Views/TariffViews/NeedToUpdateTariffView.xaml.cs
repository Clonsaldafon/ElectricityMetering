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

namespace ElectricityMetering.WPF.Views.TariffViews
{
    /// <summary>
    /// Interaction logic for NeedToUpdateTariffView.xaml
    /// </summary>
    public partial class NeedToUpdateTariffView : UserControl
    {
        public NeedToUpdateTariffView()
        {
            InitializeComponent();
        }

        private void AddTariff(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("AddTariff");

            string dateString = TextBoxDate.Text;
            string priceString = TextBoxPrice.Text;


        }
    }
}
