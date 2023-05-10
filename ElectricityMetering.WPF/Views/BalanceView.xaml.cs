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
        private readonly ApplicationContext _context = new ApplicationContext();
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

            _cellTextStyle = (Style)FindResource("LittleTableCellTextReadonly");
            _borderStyle = (Style)FindResource("LittleTableBorder");

            _borders = new Border[_rowCount, _columnCount];

            FillTable();
        }

        private void FillTable()
        {
            TextBlock textBlockDebt = new TextBlock();
            TextBlock textBlockAdvance = new TextBlock();
            TextBlock textBlockBalance = new TextBlock();

            textBlockDebt.Text = _balanceController.Debt.ToString();
            textBlockAdvance.Text = _balanceController.Advance.ToString();
            textBlockBalance.Text = _balanceController.Balance.ToString();

            textBlockDebt.Style = _cellTextStyle;
            textBlockAdvance.Style = _cellTextStyle;
            textBlockBalance.Style = _cellTextStyle;

            AddBalanceInTable(1, 0, textBlockDebt);
            AddBalanceInTable(1, 1, textBlockAdvance);
            AddBalanceInTable(1, 2, textBlockBalance);
        }

        private void AddBalanceInTable(int row, int column, TextBlock textBlock)
        {
            TableBalance.RowDefinitions.Add(new RowDefinition());

            _borders[row, column] = new Border();
            _borders[row, column].Child = textBlock;
            _borders[row, column].Style = _borderStyle;

            Grid.SetRow(_borders[row, column], row);
            Grid.SetColumn(_borders[row, column], column);

            TableBalance.Children.Add(_borders[row, column]);
        }

        private void ReloadData(object sender, RoutedEventArgs e)
        {
            FillTable();
        }
    }
}
