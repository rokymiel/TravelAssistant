﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TravelAssistant.Models;
using Xamarin.Forms;

namespace TravelAssistant.View
{
    public partial class RemindersPage : ContentPage
    {
        static ObservableCollection<Reminder> items { get; set; }
        public RemindersPage()
        {
            InitializeComponent();
            items = new ObservableCollection<Reminder>();
            App.remindersManger.GetReminder().ForEach(x => items.Add(x));
            listView.ItemsSource = items;
        }
        async void CreateNewReminder(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ReminderPage());
        }
        public static void AddReminder(Reminder reminder)
        {
            items?.Add(reminder);
            App.remindersManger.AddItem(reminder);
        }

        void RemoveReminder(System.Object sender, System.EventArgs e)
        {
            var i = ((MenuItem)sender).CommandParameter as Reminder;

            items.Remove(i);
            App.remindersManger.DeleteItem(i);
        }

        void OnReminderSelected(System.Object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {

        }
    }
}
