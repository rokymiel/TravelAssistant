using System;
using System.Collections.Generic;
using Rg.Plugins.Popup.Extensions;
using TravelAssistant.Models;
using Xamarin.Forms;

namespace TravelAssistant.View
{
    public partial class FinanceInfoPage : Rg.Plugins.Popup.Pages.PopupPage
    {
        Money Money { get; set; }
        FinancePage FinancePage { get; set; }
        public FinanceInfoPage(Money money, FinancePage financePage)
        {
            FinancePage = financePage;
            Money = money;
            InitializeComponent();
            allMoneyLabel.Text = string.Format($"{money.AllMoney:N}");
            currentMoneyLabel.Text = string.Format($"{money.CurrentMoney:N}"); ;
            spentMoneyLabel.Text = string.Format($"{money.AllMoney - money.CurrentMoney:N}");
            Animation = new Rg.Plugins.Popup.Animations.MoveAnimation();
        }
        async void deleteHistory_Clicked(object sender, EventArgs e)
        {
            App.moneyManager.DeleteAll();
            App.moneyOperationManager.DeleteAll();
            await Navigation.PopPopupAsync();
            FinancePage.CloseFinancePage();

        }


    }
}
