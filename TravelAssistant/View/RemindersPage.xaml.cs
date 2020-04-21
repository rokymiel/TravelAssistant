using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Plugin.LocalNotification;
using TravelAssistant.Managers;
using TravelAssistant.Models;
using Xamarin.Forms;

namespace TravelAssistant.View
{
    public partial class RemindersPage : ContentPage
    {
        public RemindersIDManager remindersID;
        static ObservableCollection<Reminder> items;
        public RemindersPage()
        {
            InitializeComponent();
            items = new ObservableCollection<Reminder>();
            var savedItems = App.remindersManger.GetTripItems<Reminder>(MainPage.CurrentTrip.Id);
            //var savedItems = App.remindersManger.GetReminder();
            savedItems.ForEach(x => items.Add(x));
            if (items.Count > 0)
                items = new ObservableCollection<Reminder>(items.OrderByDescending(x => x.Priority));
            remindersID = new RemindersIDManager(savedItems);
            listView.ItemsSource = items;
        }
        async void CreateNewReminder(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ReminderPage(this));
        }
        public void AddReminder(Reminder reminder)
        {
            items?.Add(reminder);
            App.remindersManger.AddItem(reminder);
            Console.WriteLine(App.remindersManger.GetTripItems<Reminder>(MainPage.CurrentTrip.Id).Count);
            items = new ObservableCollection<Reminder>(items.OrderByDescending(x => x.Priority));
            listView.ItemsSource = items;
        }

        void RemoveReminder(System.Object sender, System.EventArgs e)
        {
            DeleteReminder(((MenuItem)sender).CommandParameter as Reminder);

        }
        void DeleteReminder(Reminder reminder)
        {
            NotificationCenter.Current.Cancel(reminder.NotificationId);
            if (reminder.HasNotification)
                remindersID.DeleteId(reminder.NotificationId);
            items.Remove(reminder);
            App.remindersManger.DeleteItem(reminder);
        }
        void OnReminderSelected(System.Object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {

        }
        public static readonly BindableProperty EventNameProperty =
  BindableProperty.Create("Id", typeof(string), typeof(Reminder), null);
        void CheckBox_CheckedChanged(System.Object sender, Xamarin.Forms.CheckedChangedEventArgs e)
        {
            Console.WriteLine((sender as CheckBox).BindingContext);
            DeleteReminder((sender as CheckBox).BindingContext as Reminder);
        }
    }
}
