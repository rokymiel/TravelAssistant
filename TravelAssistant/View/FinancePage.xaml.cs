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
        public Money money;
        ObservableCollection<MoneyOperation> operations { get; set; }
        //static BindingList<MoneyOperation> bOperations = new BindingList<MoneyOperation>();
        public FinancePage()
        {
            InitializeComponent();
            operations = new ObservableCollection<MoneyOperation>();
            var list = App.moneyOperationManager.GetMoneyOperations();
            list.Reverse();
            list.ForEach(x => operations.Add(x));
            money = App.moneyManager.GetMoney();
            if (money == null)
            {
                money = new Money() { Id = Guid.NewGuid().ToString() };
                App.moneyManager.AddItem(money);
            }
            if(money.AllMoney!=0) moneyBar.Progress = ((double)money.CurrentMoney / money.AllMoney);
            currentMoneyLabel.Text = money.CurrentMoney.ToString();

            allMoneyLabel.Text = money.AllMoney.ToString();
            finEvents.ItemsSource = operations;
        }
        public void AddOperation(MoneyOperation moneyOperation)
        {
            //operations.Add(moneyOperation);
            operations.Insert(0, moneyOperation);
            App.moneyOperationManager.AddItem(moneyOperation);
            Console.WriteLine(operations.Count);
            if (moneyOperation.Type == MoneyOperation.OperationType.Add)
            {
                money.CurrentMoney += moneyOperation.Money;

                money.AllMoney += moneyOperation.Money;
                App.moneyManager.Update(money);
                allMoneyLabel.Text = money.AllMoney.ToString();
                currentMoneyLabel.Text = money.CurrentMoney.ToString();
                if (money.AllMoney != 0) moneyBar.Progress = ((double)money.CurrentMoney / money.AllMoney);

            }
            else
            {
                money.CurrentMoney -= moneyOperation.Money;
                App.moneyManager.Update(money);
                currentMoneyLabel.Text = money.CurrentMoney.ToString();
                if (money.AllMoney != 0) moneyBar.Progress = ((double)money.CurrentMoney / money.AllMoney);
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
