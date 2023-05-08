using ElectricityMetering.Core;
using ElectricityMetering.Core.Controllers;
using ElectricityMetering.Core.Models;
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
    /// Логика взаимодействия для PaymentView.xaml
    /// </summary>
    public partial class PaymentView : UserControl
    {
        private readonly Repository _repository = new Repository();

        private readonly PaymentController _paymentController = new PaymentController();

        private readonly int _rowCount;
        private readonly int _columnCount;

        private readonly Style _cellTextStyle;
        private readonly Style _borderStyle;

        private Border[,] _borders;

        public PaymentView()
        {
            InitializeComponent();

            _rowCount = _paymentController.Payments.Count + 1;
            _columnCount = TablePayment.ColumnDefinitions.Count;

            _cellTextStyle = (Style)FindResource("LittleTableCellTextReadonly");
            _borderStyle = (Style)FindResource("LittleTableBorder");

            _borders = new Border[_rowCount, _columnCount];

            FillTable();
        }

        public void FillTable()
        {
            if (_rowCount == 1)
            {
                return;
            }

            for (int row = 0; row < _paymentController.Payments.Count; row++)
            {
                Payment payment = _paymentController.Payments[row];

                TextBlock textBlockGarages = new TextBlock();
                TextBlock textBlockCash = new TextBlock();
                TextBlock textBlockNoneCash = new TextBlock();
                TextBlock textBlockDate = new TextBlock();

                textBlockGarages.Text = _paymentController.SplitBlockOfGarage(payment.Owner);
                textBlockCash.Text = payment.Cash.ToString(CultureInfo.InvariantCulture);
                textBlockNoneCash.Text = payment.NoneCash.ToString(CultureInfo.InvariantCulture);
                textBlockDate.Text = payment.Date.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);

                textBlockGarages.Style = _cellTextStyle;
                textBlockCash.Style = _cellTextStyle;
                textBlockNoneCash.Style = _cellTextStyle;
                textBlockDate.Style = _cellTextStyle;

                AddPaymentInTable(row + 1, 0, textBlockGarages);
                AddPaymentInTable(row + 1, 1, textBlockCash);
                AddPaymentInTable(row + 1, 2, textBlockNoneCash);
                AddPaymentInTable(row + 1, 3, textBlockDate);
            }
        }

        private void AddPaymentInTable(int row, int column, TextBlock textBlock)
        {
            TablePayment.RowDefinitions.Add(new RowDefinition());

            _borders[row, column] = new Border();
            _borders[row, column].Child = textBlock;
            _borders[row, column].Style = _borderStyle;

            Grid.SetRow(_borders[row, column], row);
            Grid.SetColumn(_borders[row, column], column);

            TablePayment.Children.Add(_borders[row, column]);
        }

        private void AddPaymentData(object sender, RoutedEventArgs e)
        {
            string garageNumber = TextBoxGarageNumber.Text;
            string cashPayment = TextBoxCashPayment.Text;
            string noneCashPayment = TextBoxNonCashPayment.Text;

            if (_repository.GetGarageAsync(int.Parse(garageNumber)).Result != null)
            {
                _paymentController.AddPaymentAsync(garageNumber, cashPayment, noneCashPayment);

                //FillTable();
            }
        }
    }
}
