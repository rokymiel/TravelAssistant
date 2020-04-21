using System;
using System.Collections.Generic;
using Plugin.LocalNotification;
using TravelAssistant.Models;
using Xamarin.Forms;

namespace TravelAssistant.View
{
    public partial class ReminderPage : ContentPage
    {
        private Reminder reminder;
        public ReminderPage(RemindersPage page)
        {
            Page = page;
            InitializeComponent();
            reminder = new Reminder();
        }
        private RemindersPage Page { get; set; }
        async void OnDoneClicked(object sender, EventArgs e)
        {
            bool check = !string.IsNullOrEmpty(Name.Text);
            if (check)
            {
                if (string.IsNullOrEmpty(reminder.Id))
                {
                    reminder.Id = Guid.NewGuid().ToString();
                }
                reminder.Name = Name.Text;
                reminder.Description = Description.Text;
                reminder.Priority = priority;
                reminder.TripId = MainPage.CurrentTrip.Id;
                if (styleSwitch.IsToggled)
                {

                    reminder.Date = new DateTime(DateReminder.Date.Ticks + TimeReminder.Time.Ticks);
                    if (reminder.Date >= DateTime.Now)
                    {
                        reminder.HasNotification = true;
                        reminder.NotificationId= Page.remindersID.FreeId;
                        var notification = new NotificationRequest
                        {
                            NotificationId = reminder.NotificationId,
                            Title = reminder.Name,
                            Description = GetNotificationDescription(reminder.Description),
                            // Used for Scheduling local notification, if not specified notification will show immediately.
                            NotifyTime = reminder.Date
                        };
                        NotificationCenter.Current.Show(notification);
                    }
                }


            }
            await Navigation.PopAsync();
            if (check)
            {
                Page.AddReminder(reminder);
                MessagingCenter.Send(this, "AddItem", reminder);
            }
        }
        string GetNotificationDescription(string value)
        {
            if (string.IsNullOrEmpty(value))
                return "Без описнаия.";
            else
                return value;
        }

        void Priority1_Clicked(System.Object sender, System.EventArgs e)
        {
            priority1.BackgroundColor = Color.Orange;
            priority2.BackgroundColor = Color.White;
            priority3.BackgroundColor = Color.White;
            priority4.BackgroundColor = Color.White;
            priority = 1;
        }

        void Priority2_Clicked(System.Object sender, System.EventArgs e)
        {
            priority1.BackgroundColor = Color.Orange;
            priority2.BackgroundColor = Color.Orange;
            priority3.BackgroundColor = Color.White;
            priority4.BackgroundColor = Color.White;
            priority = 2;
        }

        void Priority3_Clicked(System.Object sender, System.EventArgs e)
        {
            priority1.BackgroundColor = Color.Orange;
            priority2.BackgroundColor = Color.Orange;
            priority3.BackgroundColor = Color.Orange;
            priority4.BackgroundColor = Color.White;
            priority = 3;
        }

        void Priority4_Clicked(System.Object sender, System.EventArgs e)
        {
            priority1.BackgroundColor = Color.Orange;
            priority2.BackgroundColor = Color.Orange;
            priority3.BackgroundColor = Color.Orange;
            priority4.BackgroundColor = Color.Orange;
            priority = 4;
        }
        private int priority;
        void nonPriority_Clicked(System.Object sender, System.EventArgs e)
        {
            priority = 0;
            priority1.BackgroundColor = Color.White;
            priority2.BackgroundColor = Color.White;
            priority3.BackgroundColor = Color.White;
            priority4.BackgroundColor = Color.White;
        }
    }
}
