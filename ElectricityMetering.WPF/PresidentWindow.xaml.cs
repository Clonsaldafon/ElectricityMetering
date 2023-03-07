﻿using ElectricityMetering.Core.Controller;
using ElectricityMetering.Core.Model;
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
    /// Interaction logic for NewPresidentWindow.xaml
    /// </summary>
    public partial class PresidentWindow : Window
    {
        private Repository _repository = new Repository();

        private Garage _garage;
        private Owner _owner;
        /*private Payment _payment;
        private Tariff _tariff;*/

        public PresidentWindow()
        {
            InitializeComponent();
        }

        private void Load(object sender, RoutedEventArgs e)
        {
            string garageNumber = TextBoxGarageNumber.Text;

            if (string.IsNullOrEmpty(garageNumber))
            {
                MessageBox.Show("Введите номер гаража.");
                return;
            }

            if (!_repository.CanLoadGarage(garageNumber))
            {
                MessageBox.Show("Таких данных нет.");
                return;
            }

            _garage = _repository.LoadGarage(garageNumber);

            //_payment = _repository.LoadInfo(_owner);
            //_tariff = _repository.LoadInfo();

            // TODO: _garage.Owner is null
            //MessageBox.Show(_garage.Owner.Name);
            FillTextBoxes();
        }

        private void Add(object sender, RoutedEventArgs e)
        {
            string garageNumber = TextBoxGarageNumber.Text;

            if (!_repository.CanCreateNewGarage(garageNumber))
            {
                MessageBox.Show("Такой гараж уже существует!");
                return;
            }

            _repository.CreateNewGarage(garageNumber);
            MessageBox.Show("Гараж добавлен");
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            if (_repository.CanCreateNewOwner(_garage))
            {
                string ownerName = TextBoxOwnerName.Text;
                decimal balance = decimal.Parse(TextBoxBalance.Text);

                _repository.CreateNewOwner(ownerName, balance, _garage);
                MessageBox.Show("Владелец добавлен");
            }

            _owner = _repository.LoadOwner(_garage);

            _garage.CounterNumber = TextBoxCounterNumber.Text;
            _garage.SealNumber = TextBoxSealNumber.Text;
            _garage.SealDate = DateOnly.Parse(TextBoxSealDate.Text);
        }

        private void FillTextBoxes()
        {
            if (_owner is not null)
            {
                TextBoxBlockOfGarages.Text = GetBlockOfGarages(_owner);
                TextBoxOwnerName.Text = _owner.Name;
                TextBoxBalance.Text = _owner.Balance.ToString();
            }

            //TextBoxPaymentDate.Text = _payment.Date.ToString();
            //TextBoxCash.Text = _payment.Cash.ToString();
            //TextBoxNonCash.Text = _payment.NonCash.ToString();
            //TextBoxPaymentTotal.Text = _payment.Total.ToString();

            TextBoxCounterNumber.Text = _garage.CounterNumber;
            TextBoxSealNumber.Text = _garage.SealNumber;
            TextBoxSealDate.Text = _garage.SealDate.ToString();

            //TextBoxTariffDate.Text = _tariff.Date.ToString();
            //TextBoxTariff.Text = _tariff.Price.ToString();
            //TextBoxInPresidentWindowRemainder.Text = 
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
    }
}
