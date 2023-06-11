using ElectricityMetering.Core.Controllers;
using ElectricityMetering.Core.Models;
using ElectricityMetering.WPF.Views.MessageLogs;
using ElectricityMetering.WPF.Views.TariffViews;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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

            _cellTextStyle = (Style)FindResource(Properties.Resources.LittleTableCellTextReadonly);
            _borderStyle = (Style)FindResource(Properties.Resources.LittleTableBorder);

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

                TextBox textBoxReadonlyDate = new TextBox();
                TextBox textBoxReadonlyPrice = new TextBox();

                textBoxReadonlyDate.Text = tariff.Date.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);
                textBoxReadonlyPrice.Text = tariff.Price.ToString();

                textBoxReadonlyDate.Style = _cellTextStyle;
                textBoxReadonlyPrice.Style = _cellTextStyle;

                AddTariffInTable(row + 1, 0, textBoxReadonlyDate);
                AddTariffInTable(row + 1, 1, textBoxReadonlyPrice);
            }
        }

        private void AddTariffInTable(int row, int column, TextBox textBox)
        {
            TableTariff.RowDefinitions.Add(new RowDefinition());

            _borders[row, column] = new Border();
            _borders[row, column].Child = textBox;
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
