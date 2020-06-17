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
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace TravelAssistant.View
{

    public partial class FinancePage : ContentPage
    {
        /// <summary>
        /// Бюджет.
        /// </summary>
        public Money Money { get; set; }
        /// <summary>
        /// Список финансовых операций.
        /// </summary>
        ObservableCollection<MoneyOperation> operations;
        public FinancePage()
        {
            InitializeComponent();
            operations = new ObservableCollection<MoneyOperation>();
            var list = App.moneyOperationManager.GetTripItems<MoneyOperation>(MainPage.CurrentTrip.Id);
            list.Reverse();
            list.ForEach(x => operations.Add(x));
            var m = App.moneyManager.GetTripItems<Money>(MainPage.CurrentTrip.Id);
            if (m != null && m.Count > 0)
                Money = m[0];
            else
                Money = null;
            if (Money == null)
            {
                Money = new Money() { Id = Guid.NewGuid().ToString(), TripId = MainPage.CurrentTrip.Id };
                App.moneyManager.AddItem(Money);
            }
            if (Money.AllMoney != 0) moneyBar.Progress = ((double)Money.CurrentMoney / Money.AllMoney);
            currentMoneyLabel.Text = string.Format($"{Money.CurrentMoney:N}");
            On<iOS>().SetModalPresentationStyle(UIModalPresentationStyle.FormSheet);
            allMoneyLabel.Text = string.Format($"{Money.AllMoney:N}");
            finEvents.ItemsSource = new ObservableCollection<IGrouping<DateTime, MoneyOperation>>(operations.GroupBy(x => x.Date.Date));
        }
        public void AddOperation(MoneyOperation moneyOperation)
        {
            operations.Insert(0, moneyOperation);
            finEvents.ItemsSource = new ObservableCollection<IGrouping<DateTime, MoneyOperation>>(operations.GroupBy(x => x.Date.Date));
            App.moneyOperationManager.AddItem(moneyOperation);
            Console.WriteLine(operations.Count);
            if (moneyOperation.Type == MoneyOperation.OperationType.Add)
            {
                Money.CurrentMoney += moneyOperation.Money;

                Money.AllMoney += moneyOperation.Money;
                App.moneyManager.Update(Money);
                allMoneyLabel.Text = string.Format($"{Money.AllMoney:N}");
                currentMoneyLabel.Text = string.Format($"{Money.CurrentMoney:N}");
                if (Money.AllMoney != 0) moneyBar.Progress = ((double)Money.CurrentMoney / Money.AllMoney);

            }
            else
            {
                Money.CurrentMoney -= moneyOperation.Money;
                App.moneyManager.Update(Money);
                currentMoneyLabel.Text = string.Format($"{Money.CurrentMoney:N}");
                if (Money.AllMoney != 0) moneyBar.Progress = ((double)Money.CurrentMoney / Money.AllMoney);
            }

        }
        public async void CloseFinancePage()
        {
            await Navigation.PopAsync();
        }

        async void OnOperationAdd(System.Object sender, System.EventArgs e)
        {
           // await Navigation.PushPopupAsync(new AddOperationPopupPage(this));
            if (Device.RuntimePlatform is Device.iOS)
                await Navigation.PushModalAsync(new AddOperationPage(this));
            else
                await Navigation.PushAsync(new AddOperationPage(this));
            //await Navigation.PushAsync(new AddOperationPage(this));
        }

        void Delete_Clicked(System.Object sender, System.EventArgs e)
        {
        }

        async void Info_Clicked(System.Object sender, System.EventArgs e)
        {
            await Navigation.PushPopupAsync(new FinanceInfoPage(Money, this));
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
    public class DatetimeToStringConverter : IValueConverter
    {
        #region IValueConverter implementation

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return string.Empty;

            var datetime = (DateTime)value;
            //put your custom formatting here
            if (datetime.Date == DateTime.Now.Date)
            {
                return "Сегодня";
            }else if (datetime.AddDays(1).Date == DateTime.Now.Date)
            {
                return "Вчера";
            } else if (datetime.Year==DateTime.Now.Year)
            {
                return datetime.ToString("d MMMM");
            }
            return datetime.ToString("d MMMM, yyyy");
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

}
