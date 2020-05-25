using System;
using System.Collections.Generic;
using Rg.Plugins.Popup.Extensions;
using TravelAssistant.Models;
using Xamarin.Forms;

namespace TravelAssistant.View
{
    public partial class FinanceInfoPage : Rg.Plugins.Popup.Pages.PopupPage
    {
        /// <summary>
        /// Бюджет.
        /// </summary>
        Money Money { get; set; }
        FinancePage FinancePage { get; set; }
        public FinanceInfoPage(Money money, FinancePage financePage)
        {
            FinancePage = financePage;
            Money = money;
            InitializeComponent();
            allMoneyLabel.Text = string.Format($"{Money.AllMoney:N}");
            currentMoneyLabel.Text = string.Format($"{Money.CurrentMoney:N}"); ;
            spentMoneyLabel.Text = string.Format($"{Money.AllMoney - Money.CurrentMoney:N}");
            Animation = new Rg.Plugins.Popup.Animations.MoveAnimation();
        }
        async void deleteHistory_Clicked(object sender, EventArgs e)
        {
            App.moneyManager.DeleteAll(MainPage.CurrentTrip.Id);
            App.moneyOperationManager.DeleteAll(MainPage.CurrentTrip.Id);
            await Navigation.PopPopupAsync();
            FinancePage.CloseFinancePage();

        }


    }
}
