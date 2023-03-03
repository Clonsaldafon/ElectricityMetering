using ElectricityMetering.BL;
using ElectricityMetering.BL.Controller;
using ElectricityMetering.BL.Model;
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
using System.Windows.Shapes;

namespace ElectricityMetering.WPF
{
    /// <summary>
    /// Interaction logic for PresidentWindow.xaml
    /// </summary>
    public partial class PresidentWindow : Window
    {
        private Loader _loader = new Loader();

        private Garage _garage;
        private Owner _owner;
        private Payment _payment;
        private PricePerKw _pricePerKw;

        public PresidentWindow()
        {
            InitializeComponent();

            /*FillDataGridsColumnScheme();
            FillDataGridRowScheme(DataGridThisYear);
            FillDataGridRowScheme(DataGridOneYearAgo);
            FillDataGridRowScheme(DataGridTwoYearsAgo);*/
        }

        private void LoadInfoByGarageNumber(object sender, RoutedEventArgs e)
        {
            string garageNumber = TextBoxGarageNumber.Text;

            if (!string.IsNullOrEmpty(garageNumber))
            {
                if (_loader.CanLoadInfo(garageNumber))
                {
                    _garage = _loader.LoadInfo(garageNumber);
                    _owner = _loader.LoadInfo(_garage);
                    _payment = _loader.LoadInfo(_owner);
                    _pricePerKw = _loader.LoadInfo();

                    FillTextBoxes();
                }
                else
                {
                    MessageBox.Show("Invalid GarageNumber!");
                }
            }
            else
            {
                MessageBox.Show("GarageNumber is null!");
            }
        }

        private void FillTextBoxes()
        {
            TextBoxInPresidentWindowGarageNumber.Text = _garage.Number;
            TextBoxInPresidentWindowBlockOfGarages.Text = GetBlockOfGarages(_owner);
            TextBoxInPresidentWindowOwnerFullName.Text = _owner.FullName;
            TextBoxInPresidentWindowBalance.Text = _owner.Balance.ToString();

            TextBoxInPresidentWindowPaymentDate.Text = _payment.Date.ToString();
            TextBoxInPresidentWindowCash.Text = _payment.Cash.ToString();
            TextBoxInPresidentWindowNonCash.Text = _payment.NonCash.ToString();
            TextBoxInPresidentWindowPaymentTotal.Text = _payment.Total.ToString();

            TextBoxInPresidentWindowRemainderDate.Text = _pricePerKw.Date.ToString();
            TextBoxInPresidentWindowPricePerKw.Text = _pricePerKw.Price.ToString();
            //TextBoxInPresidentWindowRemainder.Text = 

            TextBoxInPresidentWindowCounterNumber.Text = _garage.CounterNumber;
            TextBoxInPresidentWindowSealNumber.Text = _garage.SealNumber;
            TextBoxInPresidentWindowSealDate.Text = _garage.SealingDate.ToString();
        }

        private string GetBlockOfGarages(Owner owner)
        {
            string[] garageNumbers = new string[owner.Garages.Count];

            for (int i = 0; i < owner.Garages.Count; i++)
            {
                garageNumbers[i] = owner.Garages[i].Number;
            }

            return string.Join(", ", garageNumbers);
        }

        /*private void FillDataGridsColumnScheme()
        {
            string[] labels = new[] { "Table", "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

            foreach (string label in labels)
            {
                DataGridTextColumn column = new DataGridTextColumn();
                column.Header = label;

                DataGridThisYear.Columns.Add(column);
                *//*DataGridOneYearAgo.Columns.Add(column);
                DataGridTwoYearsAgo.Columns.Add(column);*//*
            }
        }*/

        /*private void FillDataGridRowScheme(DataGrid dataGrid)
        {
            dataGrid.Items.Add(new DataGridItem { Columns = new[] { "Indications", "", "", "", "", "", "", "", "", "", "", "", "" } });
            dataGrid.Items.Add(new DataGridItem { Columns = new[] { "Consumption", "", "", "", "", "", "", "", "", "", "", "", "" } });
        }*/
    }

    /*public class DataGridItem
    {
        public string[] Columns { get; set; }
    }*/
}
