using ElectricityMetering.Core;
using ElectricityMetering.Core.Controllers;
using ElectricityMetering.Core.Models;
using ElectricityMetering.WPF.Views.MessageLogs;
using ElectricityMetering.WPF.Views.TariffViews;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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
        private readonly TariffController _tariffController = new TariffController();

        private readonly NeedToUpdateTariffView _needToUpdateTariffView = new NeedToUpdateTariffView();

        private readonly int _rowCount;
        private readonly int _columnCount;

        private readonly Style _cellTextStyle;
        private readonly Style _borderStyle;

        private readonly Border[,] _borders;

        public TariffView()
        {
            InitializeComponent();

            if (_tariffController.NeedToUpdateTariff())
            {
                MessageLog.Content = _needToUpdateTariffView;
            }

            _rowCount = _tariffController.Tariffs.Count + 1;
            _columnCount = TableTariff.ColumnDefinitions.Count;

            _cellTextStyle = (Style)FindResource("LittleTableCellTextReadonly");
            _borderStyle = (Style)FindResource("LittleTableBorder");

            _borders = new Border[_rowCount, _columnCount];

            FillTable();
        }

        private void FillTable()
        {
            if (_rowCount == 1)
            {
                return;
            }

            for (int row = 0; row < _tariffController.Tariffs.Count; row++)
            {
                Tariff tariff = _tariffController.Tariffs[row];

                TextBlock textBlockDate = new TextBlock();
                TextBlock textBlockPrice = new TextBlock();

                textBlockDate.Text = tariff.Date.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);
                textBlockPrice.Text = tariff.Price.ToString();

                textBlockDate.Style = _cellTextStyle;
                textBlockPrice.Style = _cellTextStyle;

                AddTariffInTable(row + 1, 0, textBlockDate);
                AddTariffInTable(row + 1, 1, textBlockPrice);
            }
        }

        private void AddTariffInTable(int row, int column, TextBlock textBlock)
        {
            TableTariff.RowDefinitions.Add(new RowDefinition());

            _borders[row, column] = new Border();
            _borders[row, column].Child = textBlock;
            _borders[row, column].Style = _borderStyle;

            Grid.SetRow(_borders[row, column], row);
            Grid.SetColumn(_borders[row, column], column);

            TableTariff.Children.Add(_borders[row, column]);
        }

        private void ReloadData(object sender, RoutedEventArgs e)
        {
            MessageLog.Content = new PleaseWaitTextView();
            Mouse.OverrideCursor = Cursors.Wait;

            FillTable();

            MessageLog.Content = new SuccessfulUpdateView(MessageLog);
            Mouse.OverrideCursor = null;
        }
    }
}
