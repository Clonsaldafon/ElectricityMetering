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

        TextBlock _textBlockGarages = new TextBlock();
        TextBlock _textBlockCash = new TextBlock();
        TextBlock _textBlockNoneCash = new TextBlock();
        TextBlock _textBlockDate = new TextBlock();

        public PaymentView()
        {
            InitializeComponent();

            //FillTable();
        }

        public void FillTable()
        {
            int rowCount = _paymentController.Payments.Count + 1;
            int columnCount = TablePayment.ColumnDefinitions.Count;

            if (rowCount == 1)
            {
                return;
            }

            var cellTextStyle = (Style)FindResource("LittleTableCellTextReadonly");
            var borderStyle = (Style)FindResource("LittleTableBorder");

            Border[,] borders = new Border[rowCount, columnCount];

            for (int row = 0; row < _paymentController.Payments.Count; row++)
            {
                Payment payment = _paymentController.Payments[row];

                _textBlockGarages.Text = _paymentController.SplitBlockOfGarage(payment.Owner);
                _textBlockCash.Text = payment.Cash.ToString();
                _textBlockNoneCash.Text = payment.NonCash.ToString();
                _textBlockDate.Text = payment.Date.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);

                _textBlockGarages.Style = cellTextStyle;
                _textBlockCash.Style = cellTextStyle;
                _textBlockNoneCash.Style = cellTextStyle;
                _textBlockDate.Style = cellTextStyle;

                AddPaymentInTable(row + 1, 0, borders, borderStyle, _textBlockGarages);
                AddPaymentInTable(row + 1, 1, borders, borderStyle, _textBlockCash);
                AddPaymentInTable(row + 1, 2, borders, borderStyle, _textBlockNoneCash);
                AddPaymentInTable(row + 1, 3, borders, borderStyle, _textBlockDate);
            }
        }

        private void AddPaymentInTable(int row, int column, Border[,] borders, Style borderStyle, TextBlock textBlock)
        {
            TablePayment.RowDefinitions.Add(new RowDefinition());

            borders[row, column] = new Border();
            borders[row, column].Child = textBlock;
            borders[row, column].Style = borderStyle;

            Grid.SetRow(borders[row, column], row);
            Grid.SetColumn(borders[row, column], column);

            TablePayment.Children.Add(borders[row, column]);
        }

        private void AddPaymentData(object sender, RoutedEventArgs e)
        {
            string garageNumber = TextBoxGarageNumber.Text;
            string cashPayment = TextBoxCashPayment.Text;
            string noneCashPayment = TextBoxNonCashPayment.Text;

            _paymentController.AddPaymentAsync(garageNumber, cashPayment, noneCashPayment);

            //FillTable();
        }
    }
}
