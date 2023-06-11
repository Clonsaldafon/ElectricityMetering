using ElectricityMetering.Core;
using ElectricityMetering.Core.Controllers;
using ElectricityMetering.Core.Models;
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
    /// Interaction logic for BalanceView.xaml
    /// </summary>
    public partial class BalanceView : UserControl
    {
        private readonly BalanceController _balanceController = new BalanceController();

        private readonly int _rowCount = 2;
        private readonly int _columnCount = 3;

        private readonly Style _cellTextStyle;
        private readonly Style _borderStyle;

        private readonly Border[,] _borders;

        public BalanceView()
        {
            InitializeComponent();

            _balanceController.Calculate();

            _cellTextStyle = (Style)FindResource(Properties.Resources.LittleTableCellTextReadonly);
            _borderStyle = (Style)FindResource(Properties.Resources.LittleTableBorder);

            _borders = new Border[_rowCount, _columnCount];

            FillTable();
        }

        private void FillTable()
        {
            TextBox textBoxReadonlyDebt = new TextBox();
            TextBox textBoxReadonlyAdvance = new TextBox();
            TextBox textBoxReadonlyBalance = new TextBox();

            textBoxReadonlyDebt.Text = _balanceController.Debt.ToString();
            textBoxReadonlyAdvance.Text = _balanceController.Advance.ToString();
            textBoxReadonlyBalance.Text = _balanceController.Balance.ToString();

            textBoxReadonlyDebt.Style = _cellTextStyle;
            textBoxReadonlyAdvance.Style = _cellTextStyle;
            textBoxReadonlyBalance.Style = _cellTextStyle;

            AddBalanceInTable(1, 0, textBoxReadonlyDebt);
            AddBalanceInTable(1, 1, textBoxReadonlyAdvance);
            AddBalanceInTable(1, 2, textBoxReadonlyBalance);
        }

        private void AddBalanceInTable(int row, int column, TextBox textBox)
        {
            TableBalance.RowDefinitions.Add(new RowDefinition());

            _borders[row, column] = new Border();
            _borders[row, column].Child = textBox;
            _borders[row, column].Style = _borderStyle;

            Grid.SetRow(_borders[row, column], row);
            Grid.SetColumn(_borders[row, column], column);

            TableBalance.Children.Add(_borders[row, column]);
        }

        private void ReloadData(object sender, RoutedEventArgs e)
        {
            _balanceController.Reset();
            _balanceController.Calculate();

            FillTable();
        }
    }
}
