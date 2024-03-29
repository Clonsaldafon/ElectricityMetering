﻿using ElectricityMetering.Core;
using ElectricityMetering.Core.Controllers;
using ElectricityMetering.Core.Models;
using ElectricityMetering.WPF.Views.MessageLogs;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ElectricityMetering.WPF.Views
{
    /// <summary>
    /// Interaction logic for PaymentView.xaml
    /// </summary>
    public partial class PaymentView : UserControl
    {
        private readonly PaymentController _paymentController = new PaymentController();

        private int _rowCount;
        private int _columnCount;

        private readonly Style _cellTextStyle;
        private readonly Style _borderStyle;

        private Border[,] _borders;

        public PaymentView()
        {
            InitializeComponent();

            _rowCount = _paymentController.Payments.Count + 1;
            _columnCount = TablePayment.ColumnDefinitions.Count;

            _cellTextStyle = (Style)FindResource(Properties.Resources.LittleTableCellTextReadonly);
            _borderStyle = (Style)FindResource(Properties.Resources.LittleTableBorder);

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

                TextBox textBoxReadonlyGarages = new TextBox();
                TextBox textBoxReadonlyCash = new TextBox();
                TextBox textBoxReadonlyNoneCash = new TextBox();
                TextBox textBoxReadonlyDate = new TextBox();

                textBoxReadonlyGarages.Text = _paymentController.SplitBlockOfGarages(payment.Owner);
                textBoxReadonlyCash.Text = payment.Cash.ToString(CultureInfo.InvariantCulture);
                textBoxReadonlyNoneCash.Text = payment.NoneCash.ToString(CultureInfo.InvariantCulture);
                textBoxReadonlyDate.Text = payment.Date.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);

                textBoxReadonlyGarages.Style = _cellTextStyle;
                textBoxReadonlyCash.Style = _cellTextStyle;
                textBoxReadonlyNoneCash.Style = _cellTextStyle;
                textBoxReadonlyDate.Style = _cellTextStyle;

                AddPaymentInTable(row + 1, 0, textBoxReadonlyGarages);
                AddPaymentInTable(row + 1, 1, textBoxReadonlyCash);
                AddPaymentInTable(row + 1, 2, textBoxReadonlyNoneCash);
                AddPaymentInTable(row + 1, 3, textBoxReadonlyDate);
            }
        }

        private void AddPaymentInTable(int row, int column, TextBox textBox)
        {
            TablePayment.RowDefinitions.Add(new RowDefinition());

            _borders[row, column] = new Border();
            _borders[row, column].Child = textBox;
            _borders[row, column].Style = _borderStyle;

            Grid.SetRow(_borders[row, column], row);
            Grid.SetColumn(_borders[row, column], column);

            TablePayment.Children.Add(_borders[row, column]);
        }

        private void AddPaymentData(object sender, RoutedEventArgs e)
        {
            MessageLog.Content = new PleaseWaitTextView();
            Mouse.OverrideCursor = Cursors.Wait;

            string garageNumber = TextBoxGarageNumber.Text;
            string cashPayment = TextBoxCashPayment.Text;
            string noneCashPayment = TextBoxNonCashPayment.Text;

            if (int.TryParse(garageNumber, out int number))
            {
                _ = AddPaymentAsync(number, cashPayment, noneCashPayment);
            }

            MessageLog.Content = new SuccessfulSaveView(MessageLog);
            Mouse.OverrideCursor = null;
        }

        private async Task AddPaymentAsync(int garageNumber, string cashPayment, string noneCashPayment)
        {
            if (await Repository.GetGarageAsync(garageNumber) != null)
            {
                await _paymentController.AddPaymentAsync(garageNumber, cashPayment, noneCashPayment);
            }
        }

        private void ReloadData(object sender, RoutedEventArgs e)
        {
            MessageLog.Content = new PleaseWaitTextView();
            Mouse.OverrideCursor = Cursors.Wait;

            _rowCount = _paymentController.Payments.Count + 1;
            _columnCount = TablePayment.ColumnDefinitions.Count;

            _borders = new Border[_rowCount, _columnCount];

            FillTable();

            MessageLog.Content = new SuccessfulUpdateView(MessageLog);
            Mouse.OverrideCursor = null;
        }
    }
}
