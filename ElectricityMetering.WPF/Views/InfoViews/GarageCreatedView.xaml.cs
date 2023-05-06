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

namespace ElectricityMetering.WPF.Views.InfoViews
{
    /// <summary>
    /// Логика взаимодействия для GarageCreatedView.xaml
    /// </summary>
    public partial class GarageCreatedView : UserControl
    {
        private readonly InfoView _infoView = new InfoView();

        public GarageCreatedView()
        {
            InitializeComponent();
        }

        private void CloseMessageLog(object sender, RoutedEventArgs e)
        {
            _infoView.ClearMessageLog();
        }
    }
}
