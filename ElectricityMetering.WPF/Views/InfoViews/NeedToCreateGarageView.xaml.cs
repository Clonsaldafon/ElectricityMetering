﻿using ElectricityMetering.Core.Controllers;
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

namespace ElectricityMetering.WPF.Views.InfoViews
{
    /// <summary>
    /// Логика взаимодействия для NeedToCreateGarageView.xaml
    /// </summary>
    public partial class NeedToCreateGarageView : UserControl
    {
        private readonly Controller _controller = new Controller();

        private readonly InfoView _infoView = new InfoView();

        private int _garageNumber;

        public NeedToCreateGarageView(int garageNumber)
        {
            InitializeComponent();
            _garageNumber = garageNumber;

            TextBlockGarageNumberInError.Text = _garageNumber.ToString();
        }

        private void CreateGarage(object sender, RoutedEventArgs e)
        {
            _controller.CreateGarageAsync(_garageNumber);
        }

        private void CloseMessageLog(object sender, RoutedEventArgs e)
        {
            _infoView.ClearMessageLog();
        }
    }
}
