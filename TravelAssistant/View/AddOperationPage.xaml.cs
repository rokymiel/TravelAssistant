using System;
using System.Collections.Generic;
using Plugin.Segmented.Event;
using TravelAssistant.Models;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace TravelAssistant.View
{
    public partial class AddOperationPage : ContentPage
    {
        /// <summary>
        /// Ссылка на родительскую финансовую страницу.
        /// </summary>
        FinancePage Main { get; set; }
        public AddOperationPage(FinancePage main)
        {
            On<iOS>().SetModalPresentationStyle(UIModalPresentationStyle.FormSheet);
            Main = main;
            InitializeComponent();
            SegmentedControl.OnSegmentSelected += operation_Selected;
            if (Device.RuntimePlatform == Device.iOS)
                bar.IsVisible = true;


        }
        async void OnDoneClicked(object sender, EventArgs e)
        {

            if (Device.RuntimePlatform is Device.iOS)
                await Navigation.PopModalAsync();
            else
                await Navigation.PopAsync();
            if (SegmentedControl.SelectedSegment == 0)
            {
                int value;
                if (int.TryParse(addEntry.Text, out value) && value > 0)
                {
                    MoneyOperation moneyOperation = new MoneyOperation();
                    moneyOperation.Money = value;
                    moneyOperation.Id = Guid.NewGuid().ToString();
                    moneyOperation.Type = MoneyOperation.OperationType.Add;
                    moneyOperation.Date = DateTime.Now;
                    moneyOperation.TripId = MainPage.CurrentTrip.Id;
                    Main.AddOperation(moneyOperation);

                }

            }
            if (SegmentedControl.SelectedSegment == 1 && Main.Money.CurrentMoney > 0 && !string.IsNullOrEmpty(minusDesrEntry.Text))
            {
                int value;
                if (int.TryParse(minusEntry.Text, out value) && value > 0)
                {
                    if (value <= Main.Money.CurrentMoney)
                    {
                        MoneyOperation moneyOperation = new MoneyOperation();
                        moneyOperation.Money = value;
                        moneyOperation.Description = minusDesrEntry.Text;
                        moneyOperation.Id = Guid.NewGuid().ToString();
                        moneyOperation.Type = MoneyOperation.OperationType.Minus;
                        moneyOperation.Date = DateTime.Now;
                        moneyOperation.TripId = MainPage.CurrentTrip.Id;
                        Main.AddOperation(moneyOperation);
                    }
                }
            }
        }

        void addEntry_TextChanged(System.Object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            int value;
            if (string.IsNullOrEmpty(e.NewTextValue) || int.TryParse(e.NewTextValue, out value) && value > 0)
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
            if (string.IsNullOrEmpty(e.NewTextValue) || int.TryParse(e.NewTextValue, out value) && value > 0)
            {
                wrongMinusFormat.IsVisible = false;
            }
            else
            {
                wrongMinusFormat.IsVisible = true;
            }
        }

        void operation_Selected(object sender, SegmentSelectEventArgs e)
        {
            addLayout.IsVisible = e.NewValue == 0 ? true : false;
            minusLayout.IsVisible = e.NewValue == 0 ? false : true;
        }
    }
}
