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

        public PaymentView()
        {
            InitializeComponent();

            FillTable();
        }

        public void FillTable()
        {
            int rowCount = _repository.GetPayments().Count;
            int columnCount = TablePayment.ColumnDefinitions.Count;

            var cellTextStyle = (Style)FindResource("BigTableCellTextReadonly");
            var borderStyle = (Style)FindResource("BigTableBorder");

            Border[,] borders = new Border[rowCount, columnCount];
            TextBlock[,] textBlocks = new TextBlock[rowCount, columnCount];

            TextBlock textBlockGarages = new TextBlock();
            TextBlock textBlockCash = new TextBlock();
            TextBlock textBlockNoneCash = new TextBlock();
            TextBlock textBlockDate = new TextBlock();

            textBlockGarages.Text = _paymentController.BlockOfGarages;
            textBlockCash.Text = _paymentController.Cash.ToString();
            textBlockNoneCash.Text = _paymentController.NoneCash.ToString();
            textBlockDate.Text = _paymentController.Date.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);

            textBlocks[0, 0] = textBlockGarages;
            textBlocks[0, 1] = textBlockCash;
            textBlocks[0, 2] = textBlockNoneCash;
            textBlocks[0, 3] = textBlockDate;

            for (int i = 0; i < rowCount; i++)
            {
                TablePayment.RowDefinitions.Add(new RowDefinition());

                for (int j = 0; j < columnCount; j++)
                {
                    borders[i, j] = new Border();
                    borders[i, j].Child = textBlocks[i, j];
                    borders[i, j].Style = borderStyle;

                    Grid.SetRow(borders[i, j], rowCount);
                    Grid.SetColumn(borders[i, j], j);

                    TablePayment.Children.Add(borders[i, j]);
                }
            }
        }

        private void AddPaymentData(object sender, RoutedEventArgs e)
        {
            string garageNumber = TextBoxGarageNumber.Text;
            string cashPayment = TextBoxCashPayment.Text;
            string noneCashPayment = TextBoxNonCashPayment.Text;

            _paymentController.AddPaymentAsync(garageNumber, cashPayment, noneCashPayment);

            FillTable();
        }
    }
}
