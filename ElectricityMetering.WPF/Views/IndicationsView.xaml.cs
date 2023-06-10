using ElectricityMetering.Core.Controllers;
using SciChart.Core.Extensions;
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
    /// Interaction logic for IndicationsView.xaml
    /// </summary>
    public partial class IndicationsView : UserControl
    {
        private const int _countOfTables = 3;
        private const int _countOfInfoColumns = 5;
        private const int _countOfMonths = 12;

        private readonly IndicationsController _indicationsController = new IndicationsController();

        private int _rowCount;
        private int _columnCount;

        private readonly Style _cellTextReadonlyStyle;
        private readonly Style _cellTextStyle;
        private readonly Style _borderStyle;

        private Border[,] _bordersNow;

        private Grid[] _tables;
        private TextBox[] _textBoxesInfo;
        private TextBox[] _textBoxesMonths;

        public IndicationsView()
        {
            InitializeComponent();

            if (_indicationsController.YearChanged)
            {
                ChangeYears(DateTime.Now.Year);
            }

            if (_indicationsController.InfoData.Count > 0)
            {
                _rowCount = _indicationsController.InfoData.Count + 1;
                _columnCount = _countOfInfoColumns + _countOfMonths;

                _cellTextReadonlyStyle = (Style)FindResource("BigTableCellTextReadonly");
                _cellTextStyle = (Style)FindResource("BigTableCellText");
                _borderStyle = (Style)FindResource("BigTableBorder");

                _bordersNow = new Border[_rowCount, _columnCount];

                _tables = new Grid[_countOfTables]
                {
                    TableIndicationsNow,
                    TableIndicationsOneYearAgo,
                    TableIndicationsTwoYearsAgo
                };

                FillTables();
            }
        }

        private void ChangeYears(int currentYear)
        {
            ItemCollection tabItems = TabControlIndications.Items;

            ((TabItem)tabItems[0]).Header = currentYear.ToString();
            ((TabItem)tabItems[1]).Header = (currentYear - 1).ToString();
            ((TabItem)tabItems[2]).Header = (currentYear - 2).ToString();
        }

        private void FillTables()
        {
            if (_rowCount == 1)
            {
                return;
            }

            for (int row = 1; row < _rowCount; row++)
            {
                _textBoxesInfo = new TextBox[_countOfInfoColumns]
                {
                    new TextBox(),
                    new TextBox(),
                    new TextBox(),
                    new TextBox(),
                    new TextBox()
                };

                foreach (TextBox textBox in _textBoxesInfo)
                {
                    textBox.Style = _cellTextReadonlyStyle;
                }

                _textBoxesMonths = new TextBox[_countOfMonths]
                {
                    new TextBox(), new TextBox(), new TextBox(),
                    new TextBox(), new TextBox(), new TextBox(),
                    new TextBox(), new TextBox(), new TextBox(),
                    new TextBox(), new TextBox(), new TextBox()
                };

                foreach (TextBox textBox in _textBoxesMonths)
                {
                    textBox.Style = _cellTextReadonlyStyle;
                }

                for (int i = 0; i < _textBoxesInfo.Length; i++)
                {
                    _textBoxesInfo[i].Text = _indicationsController.InfoData[row - 1][i];
                }

                for (int i = 0; i < _textBoxesInfo.Length; i++)
                {
                    AddEntryInTable(TableIndicationsNow, row, i, _textBoxesInfo[i]);
                }

                _textBoxesInfo = new TextBox[_countOfInfoColumns]
                {
                    new TextBox(),
                    new TextBox(),
                    new TextBox(),
                    new TextBox(),
                    new TextBox()
                };

                foreach (TextBox textBox in _textBoxesInfo)
                {
                    textBox.Style = _cellTextReadonlyStyle;
                }

                for (int i = 0; i < _textBoxesInfo.Length; i++)
                {
                    _textBoxesInfo[i].Text = _indicationsController.InfoData[row - 1][i];
                }

                for (int i = 0; i < _textBoxesInfo.Length; i++)
                {
                    AddEntryInTable(TableIndicationsOneYearAgo, row, i, _textBoxesInfo[i]);
                }

                _textBoxesInfo = new TextBox[_countOfInfoColumns]
                {
                    new TextBox(),
                    new TextBox(),
                    new TextBox(),
                    new TextBox(),
                    new TextBox()
                };

                foreach (TextBox textBox in _textBoxesInfo)
                {
                    textBox.Style = _cellTextReadonlyStyle;
                }

                for (int i = 0; i < _textBoxesInfo.Length; i++)
                {
                    _textBoxesInfo[i].Text = _indicationsController.InfoData[row - 1][i];
                }

                for (int i = 0; i < _textBoxesInfo.Length; i++)
                {
                    AddEntryInTable(TableIndicationsTwoYearsAgo, row, i, _textBoxesInfo[i]);
                }

                for (int column = 0; column < _textBoxesMonths.Length; column++)
                {
                    _textBoxesMonths[column].Text = _indicationsController.IndicationsNow[row - 1][column];
                    _textBoxesMonths[column].Style = _cellTextStyle;

                    AddEntryInTable(TableIndicationsNow, row, column + 5, _textBoxesMonths[column]);
                }

                _textBoxesMonths = new TextBox[_countOfMonths]
                {
                    new TextBox(), new TextBox(), new TextBox(),
                    new TextBox(), new TextBox(), new TextBox(),
                    new TextBox(), new TextBox(), new TextBox(),
                    new TextBox(), new TextBox(), new TextBox()
                };

                foreach (TextBox textBox in _textBoxesMonths)
                {
                    textBox.Style = _cellTextReadonlyStyle;
                }

                for (int column = 0; column < _textBoxesMonths.Length; column++)
                {
                    _textBoxesMonths[column].Text = _indicationsController.IndicationsOneYearAgo[row - 1][column];
                    _textBoxesMonths[column].Style = _cellTextReadonlyStyle;

                    AddEntryInTable(TableIndicationsOneYearAgo, row, column + 5, _textBoxesMonths[column]);
                }

                _textBoxesMonths = new TextBox[_countOfMonths]
                {
                    new TextBox(), new TextBox(), new TextBox(),
                    new TextBox(), new TextBox(), new TextBox(),
                    new TextBox(), new TextBox(), new TextBox(),
                    new TextBox(), new TextBox(), new TextBox()
                };

                foreach (TextBox textBox in _textBoxesMonths)
                {
                    textBox.Style = _cellTextReadonlyStyle;
                }

                for (int column = 0; column < _textBoxesMonths.Length; column++)
                {
                    _textBoxesMonths[column].Text = _indicationsController.IndicationsTwoYearsAgo[row - 1][column];
                    _textBoxesMonths[column].Style = _cellTextReadonlyStyle;

                    AddEntryInTable(TableIndicationsTwoYearsAgo, row, column + 5, _textBoxesMonths[column]);
                }
            }
        }

        private void AddEntryInTable(Grid table, int row, int column, TextBox textBox)
        {
            table.RowDefinitions.Add(new RowDefinition());

            _bordersNow[row, column] = new Border();
            _bordersNow[row, column].Style = _borderStyle;

            if (_bordersNow[row, column].Parent != null)
            {
                ((Grid)_bordersNow[row, column].Parent).Children.Remove(_bordersNow[row, column]);
            }

            _bordersNow[row, column].Child = textBox;

            Grid.SetRow(_bordersNow[row, column], row);
            Grid.SetColumn(_bordersNow[row, column], column);

            table.Children.Add(_bordersNow[row, column]);
        }

        private void SaveIndications(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;

            Dictionary<string, List<string>> indicationsByCounterNumber = new Dictionary<string, List<string>>();

            int columnCounterIndex = 2;

            int minColumnIndications = 5;
            int maxColumnIndications = 16;

            string currentCounterNumber = "";
            List<string> indications = new List<string>();

            for (int row = 0; row < _rowCount; row++)
            {
                if (TableIndicationsNow.Children[columnCounterIndex] is Border borderCounter)
                {
                    if (borderCounter.Child is TextBox textBoxCounter)
                    {
                        currentCounterNumber = textBoxCounter.Text;
                    }
                }

                for (int column = minColumnIndications; column < maxColumnIndications + 1; column++)
                {
                    if (TableIndicationsNow.Children[column] is Border borderIndication)
                    {
                        if (borderIndication.Child is TextBox textBoxIndication)
                        {
                            indications.Add(textBoxIndication.Text);
                        }
                    }
                }

                if (!currentCounterNumber.IsNullOrEmpty() && indications.Count != 0)
                {
                    indicationsByCounterNumber[currentCounterNumber] = indications;
                    indications = new List<string>();
                }

                columnCounterIndex += 17;
                minColumnIndications += 17;
                maxColumnIndications += 17;
            }

            _ = _indicationsController.SaveDataAsync(indicationsByCounterNumber);

            Mouse.OverrideCursor = null;
        }
    }
}
