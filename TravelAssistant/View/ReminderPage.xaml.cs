using System;
using System.Collections.Generic;
using TravelAssistant.Models;
using Xamarin.Forms;

namespace TravelAssistant.View
{
    public partial class ReminderPage : ContentPage
    {
        private Reminder reminder;
        public ReminderPage()
        {
            InitializeComponent();
            reminder = new Reminder();
        }
        async void OnDoneClicked(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(Name.Text))
            {
                if (string.IsNullOrEmpty(reminder.Id))
                {
                    reminder.Id = Guid.NewGuid().ToString();
                }
                reminder.Name = Name.Text;
                reminder.Description = Description.Text;
                if (styleSwitch.IsToggled)
                {

                    reminder.Date = new DateTime(DateReminder.Date.Ticks + TimeReminder.Time.Ticks);
                }

                RemindersPage.AddReminder(reminder);
            }
            await Navigation.PopAsync();
        }


    }
}
