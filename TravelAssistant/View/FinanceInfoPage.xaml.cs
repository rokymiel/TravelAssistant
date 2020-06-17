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
        protected override void OnAppearing()
        {
            base.OnAppearing();
            cakeView.HeightRequest = GetHeight;
            Console.WriteLine(GetHeight);
        }
        protected override void OnAppearingAnimationBegin()
        {
            cakeView.HeightRequest = GetHeight;
            Console.WriteLine(GetHeight);
            base.OnAppearingAnimationBegin();
        }
        async void deleteHistory_Clicked(object sender, EventArgs e)
        {
            App.moneyManager.DeleteAll(MainPage.CurrentTrip.Id);
            App.moneyOperationManager.DeleteAll(MainPage.CurrentTrip.Id);
            await Navigation.PopPopupAsync();
            FinancePage.CloseFinancePage();

        }
        double savedTranslationY;
        double lastTranslationY;
        double GetHeight => page.Height * 0.8;
        async void PanGestureRecognizer_PanUpdated(System.Object sender, Xamarin.Forms.PanUpdatedEventArgs e)
        {

            switch (e.StatusType)
            {

                case GestureStatus.Running:

                    //Console.WriteLine(e.TotalY);
                    if (e.TotalY + savedTranslationY > 0)
                    {
                        cakeView.TranslationY = e.TotalY + savedTranslationY;
                        lastTranslationY = cakeView.TranslationY;
                        //Math.Max(Math.Min(0, cakeView.Y + e.TotalY),-Math.Abs(Content.Height - page.Height));
                    }
                    break;
                case GestureStatus.Completed:
                    savedTranslationY = lastTranslationY;
                    if (savedTranslationY <= GetHeight * 0.14)
                    {
                        savedTranslationY = cakeView.TranslationY = 0;
                    }
                    else
                    {
                        await Navigation.PopPopupAsync();
                    }

                    //Console.WriteLine("Completed");
                    break;
            }
        }

    }
}
