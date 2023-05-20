using ElectricityMetering.Core.Controllers;
using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для IndicationsView.xaml
    /// </summary>
    public partial class IndicationsView : UserControl
    {
        private readonly IndicationsController _indicationsController;

        private int _rowCount;
        private int _columnCount;

        private readonly Style _cellTextReadonlyStyle;
        private readonly Style _cellTextStyle;
        private readonly Style _borderStyle;

        private Border[,] _borders;

        private int _nowYear = DateTime.Now.Year;

        private List<Grid> _tables;

        public IndicationsView()
        {
            InitializeComponent();

            _indicationsController = new IndicationsController(_nowYear);

            if (_indicationsController.Data.Count > 0)
            {
                _rowCount = _indicationsController.Data.Count + 1;
                _columnCount = _indicationsController.Data[0].Count;

                _cellTextReadonlyStyle = (Style)FindResource("BigTableCellTextReadonly");
                _cellTextStyle = (Style)FindResource("BigTableCellText");
                _borderStyle = (Style)FindResource("BigTableBorder");

                _borders = new Border[_rowCount, _columnCount];

                _tables = new List<Grid>()
                {
                    TableIndicationsNow,
                    TableIndicationsOneYearAgo,
                    TableIndicationsTwoYearsAgo
                };

                FillTables();
            }
        }

        private void FillTables()
        {
            if (_rowCount == 1)
            {
                return;
            }

            TextBlock textBlockGarages = new TextBlock();
            TextBlock textBlockOwner = new TextBlock();
            TextBlock textBlockCounter = new TextBlock();
            TextBlock textBlockSeal = new TextBlock();
            TextBlock textBlockSealDate = new TextBlock();

            textBlockGarages.Style = _cellTextReadonlyStyle;
            textBlockOwner.Style = _cellTextReadonlyStyle;
            textBlockCounter.Style = _cellTextReadonlyStyle;
            textBlockSeal.Style = _cellTextReadonlyStyle;
            textBlockSealDate.Style = _cellTextReadonlyStyle;

            List<TextBlock> textBlocksMonths = new List<TextBlock>()
            {
                new TextBlock(), new TextBlock(), new TextBlock(),
                new TextBlock(), new TextBlock(), new TextBlock(),
                new TextBlock(), new TextBlock(), new TextBlock(),
                new TextBlock(), new TextBlock(), new TextBlock()
            };

            for (int row = 0; row < _rowCount; row++)
            {
                textBlockGarages.Text = _indicationsController.Data[row][0];
                textBlockOwner.Text = _indicationsController.Data[row][1];
                textBlockCounter.Text = _indicationsController.Data[row][2];
                textBlockSeal.Text = _indicationsController.Data[row][3];
                textBlockSealDate.Text = _indicationsController.Data[row][4];

                foreach (Grid table in _tables)
                {
                    AddEntryInTable(table, row, 0, textBlockGarages);
                    AddEntryInTable(table, row, 1, textBlockOwner);
                    AddEntryInTable(table, row, 2, textBlockCounter);
                    AddEntryInTable(table, row, 3, textBlockSeal);
                    AddEntryInTable(table, row, 4, textBlockSealDate);
                }

                for (int column = 0; column < textBlocksMonths.Count; column++)
                {
                    textBlocksMonths[column].Text = _indicationsController.Data[row][column + 5];
                    textBlocksMonths[column].Style = _cellTextStyle;

                    AddEntryInTable(TableIndicationsNow, row, column + 5, textBlocksMonths[column]);
                }

                for (int column = 0; column < textBlocksMonths.Count; column++)
                {
                    textBlocksMonths[column].Text = _indicationsController.Data[row][column + 5];
                    textBlocksMonths[column].Style = _cellTextReadonlyStyle;

                    AddEntryInTable(TableIndicationsOneYearAgo, row, column + 5, textBlocksMonths[column]);
                }

                for (int column = 0; column < textBlocksMonths.Count; column++)
                {
                    textBlocksMonths[column].Text = _indicationsController.Data[row][column + 5];
                    textBlocksMonths[column].Style = _cellTextReadonlyStyle;

                    AddEntryInTable(TableIndicationsTwoYearsAgo, row, column + 5, textBlocksMonths[column]);
                }
            }
        }

        private void AddEntryInTable(Grid table, int row, int column, TextBlock textBlock)
        {
            table.RowDefinitions.Add(new RowDefinition());

            _borders[row, column] = new Border();
            _borders[row, column].Child = textBlock;
            _borders[row, column].Style = _borderStyle;

            Grid.SetRow(_borders[row, column], row);
            Grid.SetColumn(_borders[row, column], column);

            table.Children.Add(_borders[row, column]);
        }
    }
}
