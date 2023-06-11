using ElectricityMetering.Core.Controllers;
using System.Windows;
using System.Windows.Controls;

namespace ElectricityMetering.WPF.Views.TariffViews
{
    /// <summary>
    /// Interaction logic for NeedToUpdateTariffView.xaml
    /// </summary>
    public partial class NeedToUpdateTariffView : UserControl
    {
        private readonly TariffController _tariffController = new TariffController();

        public NeedToUpdateTariffView()
        {
            InitializeComponent();
        }

        private void AddTariff(object sender, RoutedEventArgs e)
        {
            string dateString = TextBoxDate.Text;
            string priceString = TextBoxPrice.Text;

            _ = _tariffController.AddTariffAsync(dateString, priceString);
        }
    }
}
