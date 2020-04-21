using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.PancakeView;
using TravelAssistant.Models;
using System.ComponentModel;
using System.Linq;
using Rg.Plugins.Popup.Extensions;
using TravelAssistant.Managers;

namespace TravelAssistant.View
{

    public partial class FinancePage : ContentPage
    {
        public Money money;
        ObservableCollection<MoneyOperation> operations { get; set; }
        public FinancePage()
        {
            InitializeComponent();
            operations = new ObservableCollection<MoneyOperation>();
            var list = App.moneyOperationManager.GetTripItems<MoneyOperation>(MainPage.CurrentTrip.Id);
            list.Reverse();
            list.ForEach(x => operations.Add(x));
            var m = App.moneyManager.GetTripItems<Money>(MainPage.CurrentTrip.Id);
            if (m != null && m.Count > 0)
                money = m[0];
            else
                money = null;
            if (money == null)
            {
                money = new Money() { Id = Guid.NewGuid().ToString(), TripId=MainPage.CurrentTrip.Id };
                App.moneyManager.AddItem(money);
            }
            if (money.AllMoney != 0) moneyBar.Progress = ((double)money.CurrentMoney / money.AllMoney);
            currentMoneyLabel.Text = string.Format($"{money.CurrentMoney:N}");

            allMoneyLabel.Text = string.Format($"{money.AllMoney:N}");
            finEvents.ItemsSource = operations;
        }
        public void AddOperation(MoneyOperation moneyOperation)
        {
            operations.Insert(0, moneyOperation);
            App.moneyOperationManager.AddItem(moneyOperation);
            Console.WriteLine(operations.Count);
            if (moneyOperation.Type == MoneyOperation.OperationType.Add)
            {
                money.CurrentMoney += moneyOperation.Money;

                money.AllMoney += moneyOperation.Money;
                App.moneyManager.Update(money);
                allMoneyLabel.Text = string.Format($"{money.AllMoney:N}");
                currentMoneyLabel.Text = string.Format($"{money.CurrentMoney:N}");
                if (money.AllMoney != 0) moneyBar.Progress = ((double)money.CurrentMoney / money.AllMoney);

            }
            else
            {
                money.CurrentMoney -= moneyOperation.Money;
                App.moneyManager.Update(money);
                currentMoneyLabel.Text = string.Format($"{money.CurrentMoney:N}");
                if (money.AllMoney != 0) moneyBar.Progress = ((double)money.CurrentMoney / money.AllMoney);
            }

        }
        public async void CloseFinancePage()
        {
            await Navigation.PopAsync();
        }

        async void OnOperationAdd(System.Object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new AddOperationPage(this));
        }

        void Delete_Clicked(System.Object sender, System.EventArgs e)
        {
        }

        async void Info_Clicked(System.Object sender, System.EventArgs e)
        {
            await Navigation.PushPopupAsync(new FinanceInfoPage(money, this));
        }

        async void finEvents_SelectionChanged(System.Object sender, Xamarin.Forms.SelectionChangedEventArgs e)
        {
            var i = e.CurrentSelection.FirstOrDefault() as MoneyOperation;
            if (i != null)
            {
                await Navigation.PushPopupAsync(new OperationDetailsPage(i));
            }
            (sender as CollectionView).SelectedItem = null;
        }

        async void Operation_Tapped(System.Object sender, System.EventArgs e)
        {
            var item = (MoneyOperation)(sender as PancakeView).BindingContext;
            if (item != null)
            {
                (sender as PancakeView).IsEnabled = false;
                await AnimationManager.StartScalePancakeView(sender as PancakeView);

                await Navigation.PushPopupAsync(new OperationDetailsPage(item));
                (sender as PancakeView).IsEnabled = true;
            }


        }
    }

}
