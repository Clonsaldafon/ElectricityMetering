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

        private readonly int _rowCount = 1;
        private readonly int _columnCount = 3;

        public BalanceView()
        {
            InitializeComponent();
            _balanceController.Calculate();

            var cellTextStyle = (Style)FindResource("LittleTableCellTextReadonly");
            var borderStyle = (Style)FindResource("LittleTableBorder");

            Border[,] borders = new Border[_rowCount, _columnCount];
            TextBlock[,] textBlocks = new TextBlock[_rowCount, _columnCount];

            TextBlock textBlockDebt = new TextBlock();
            TextBlock textBlockAdvance = new TextBlock();
            TextBlock textBlockBalance = new TextBlock();

            textBlockDebt.Text = _balanceController.Debt.ToString();
            textBlockAdvance.Text = _balanceController.Advance.ToString();
            textBlockBalance.Text = _balanceController.Balance.ToString();

            textBlockDebt.Style = cellTextStyle;
            textBlockAdvance.Style = cellTextStyle;
            textBlockBalance.Style = cellTextStyle;

            textBlocks[0, 0] = textBlockDebt;
            textBlocks[0, 1] = textBlockAdvance;
            textBlocks[0, 2] = textBlockBalance;

            for (int i = 0; i < _rowCount; i++)
            {
                TableBalance.RowDefinitions.Add(new RowDefinition());

                for (int j = 0; j < _columnCount; j++)
                {
                    borders[i, j] = new Border();
                    borders[i, j].Child = textBlocks[i, j];
                    borders[i, j].Style = borderStyle;

                    Grid.SetRow(borders[i, j], _rowCount);
                    Grid.SetColumn(borders[i, j], j);

                    TableBalance.Children.Add(borders[i, j]);
                }
            }
        }
    }
}
