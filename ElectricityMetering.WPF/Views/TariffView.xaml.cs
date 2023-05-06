using ElectricityMetering.Core;
using ElectricityMetering.Core.Controllers;
using ElectricityMetering.Core.Models;
using ElectricityMetering.WPF.Views.TariffViews;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace ElectricityMetering.WPF.Views
{
    /// <summary>
    /// Interaction logic for TariffView.xaml
    /// </summary>
    public partial class TariffView : UserControl
    {
        private readonly ApplicationContext _context = new ApplicationContext();
        private readonly TariffController _tariffController = new TariffController();

        public TariffView()
        {
            InitializeComponent();

            // TODO:
            /*for (int i = 0; i < numberOfRowsToAdd; i++)
            {
                myGrid.RowDefinitions.Add(new RowDefinition());
            }*/
        }
    }
}
