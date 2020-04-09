using System;
using System.Collections.Generic;
using TravelAssistant.Models;

using Xamarin.Forms;

namespace TravelAssistant.View
{
    public partial class AddOperationPage : ContentPage
    {
        FinancePage Main;
        public AddOperationPage(FinancePage main)
        {
            Main = main;
            InitializeComponent();
        }
        async void OnDoneClicked(object sender, EventArgs e)
        {

            await Navigation.PopAsync();
            if (addCheckBox.IsChecked) {
                int value;
                if (int.TryParse(addEntry.Text, out value))
                {
                    MoneyOperation moneyOperation = new MoneyOperation();
                    moneyOperation.Money = value;
                    moneyOperation.Id = Guid.NewGuid().ToString();
                    moneyOperation.Type = MoneyOperation.OperationType.Add;
                    moneyOperation.Date = DateTime.Now;
                    Main.AddOperation(moneyOperation);
                    
                }
                
            }
            if (minusCheckBox.IsChecked&&Main.money.CurrentMoney>0&& !string.IsNullOrEmpty(minusDesrEntry.Text))
            {
                int value;
                if (int.TryParse(minusEntry.Text, out value))
                {
                    if (value<=Main.money.CurrentMoney) {
                        MoneyOperation moneyOperation = new MoneyOperation();
                        moneyOperation.Money = value;
                        moneyOperation.Description = minusDesrEntry.Text;
                        moneyOperation.Id = Guid.NewGuid().ToString();
                        moneyOperation.Type = MoneyOperation.OperationType.Minus;
                        moneyOperation.Date = DateTime.Now;
                        Main.AddOperation(moneyOperation);
                    }
                }
            }
        }

        void addEntry_TextChanged(System.Object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            int value;
            if (string.IsNullOrEmpty(e.NewTextValue)||int.TryParse(e.NewTextValue, out value))
            {
                wrongAddFormat.IsVisible = false;
            }
            else
            {
                wrongAddFormat.IsVisible = true;
            }

        }

        void minusEntry_TextChanged(System.Object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            int value;
            if (string.IsNullOrEmpty(e.NewTextValue)||int.TryParse(e.NewTextValue, out value))
            {
                wrongMinusFormat.IsVisible = false;
            }
            else
            {
                wrongMinusFormat.IsVisible = true;
            }
        }

        void addCheckBox_CheckedChanged(System.Object sender, Xamarin.Forms.CheckedChangedEventArgs e)
        {
            if (e.Value)
            {
                minusCheckBox.IsChecked = false;
            }
        }

        void minusCheckBox_CheckedChanged(System.Object sender, Xamarin.Forms.CheckedChangedEventArgs e)
        {
            if (e.Value)
            {
                addCheckBox.IsChecked = false;
            }
        }

        void addLabel_Tapped(System.Object sender, System.EventArgs e)
        {

            addCheckBox.IsChecked = !addCheckBox.IsChecked;
        }
        void minusLabel_Tapped(System.Object sender, System.EventArgs e)
        {
            minusCheckBox.IsChecked = !minusCheckBox.IsChecked;
        }
    }
}
