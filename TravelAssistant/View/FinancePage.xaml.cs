using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using TravelAssistant.Models;
using System.ComponentModel;
using System.Linq;

namespace TravelAssistant.View
{

    public partial class FinancePage : ContentPage
    {
        public int currentMoney;
        public int allMoney;
        ObservableCollection<MoneyOperation> operations { get; set; }
        //static BindingList<MoneyOperation> bOperations = new BindingList<MoneyOperation>();
        public FinancePage()
        {
            InitializeComponent();
            operations = new ObservableCollection<MoneyOperation>();
            
            finEvents.ItemsSource = operations;
            finEvents.IsVisible = true;
        }
        public void AddOperation(MoneyOperation moneyOperation)
        {
            //operations.Add(moneyOperation);
            operations.Insert(0, moneyOperation);
            Console.WriteLine(operations.Count);
            if (moneyOperation.Type == MoneyOperation.OperationType.Add)
            {
                currentMoney += moneyOperation.Money;
                allMoney += moneyOperation.Money;
                allMoneyLabel.Text = allMoney.ToString();
                currentMoneyLabel.Text = currentMoney.ToString();
                moneyBar.Progress = ((double)currentMoney / allMoney);

            }
            else
            {
                currentMoney -= moneyOperation.Money;
                currentMoneyLabel.Text = currentMoney.ToString();
                moneyBar.Progress = ((double)currentMoney / allMoney);
            }


            //operations = new ObservableCollection<MoneyOperation>(operations.Reverse());
            //finEvents.ItemsSource = operations;
            //finEvents.ItemsSource = operations;
            // operations.

        }
        async void OnOperationAdd(System.Object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new AddOperationPage(this));
            //MoneyOperation moneyOperation2 = new MoneyOperation();
            //moneyOperation2.Money = 1231;
            //moneyOperation2.Id = Guid.NewGuid().ToString();
            //moneyOperation2.Type = MoneyOperation.OperationType.Add;
            //operations.Add(moneyOperation2);
            //operations.Add(moneyOperation2);
        }
    }

}
