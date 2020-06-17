using System;
using System.Collections.Generic;
using TravelAssistant.Models;
using Xamarin.Forms;
using Xamarin.Essentials;
using Rg.Plugins.Popup.Extensions;

namespace TravelAssistant.View
{
    public partial class OperationDetailsPage : Rg.Plugins.Popup.Pages.PopupPage
    {
        /// <summary>
        /// Выбранная операция.
        /// </summary>
        MoneyOperation Money { get; set; }
        public OperationDetailsPage(MoneyOperation money)
        {
            Money = money;
            InitializeComponent();
            Animation = new Rg.Plugins.Popup.Animations.MoveAnimation();
            cakeView.BindingContext = Money;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            cakeView.HeightRequest = GetHeight;
            //Console.WriteLine(GetHeight);
        }
        protected override void OnAppearingAnimationBegin()
        {
            cakeView.HeightRequest = GetHeight;
            Console.WriteLine(GetHeight);
            base.OnAppearingAnimationBegin();
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
