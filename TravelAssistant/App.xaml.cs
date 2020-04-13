using System;
using Plugin.LocalNotification;
using TravelAssistant.Managers;
using TravelAssistant.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TravelAssistant
{
    public partial class App : Application
    {
        string dbNotesPath => FileAccessManager.GetLocalFilePath("notes.db3");
        string dbRemindersPath => FileAccessManager.GetLocalFilePath("reminders.db3");
        string dbMoneyPath => FileAccessManager.GetLocalFilePath("money.db3");
        string dbMoneyOperationsPath => FileAccessManager.GetLocalFilePath("moneyOperations.db3");
        string dbDocumentsPath => FileAccessManager.GetLocalFilePath("notes.db3");
        string dbPlacesPath => FileAccessManager.GetLocalFilePath("placess.db3");
        public static SQLManger<Note> notesManger;
        public static SQLManger<Reminder> remindersManger;
        public static SQLManger<Money> moneyManager;
        public static SQLManger<MoneyOperation> moneyOperationManager;
        public static SQLManger<Document> documentManager;
        public static SQLManger<Place> placesManager;
        public App()
        {
            InitializeComponent();
            notesManger = new SQLManger<Note>(dbNotesPath);
            remindersManger = new SQLManger<Reminder>(dbRemindersPath);
            moneyManager = new SQLManger<Money>(dbMoneyPath);
            moneyOperationManager = new SQLManger<MoneyOperation>(dbMoneyOperationsPath);
            documentManager = new SQLManger<Document>(dbDocumentsPath);
            placesManager = new SQLManger<Place>(dbPlacesPath);
            NotificationCenter.Current.NotificationTapped += OnLocalNotificationTapped;
            //MainPage = new MainPage();
            MainPage = new NavigationPage(new AllTripsPage());
        }
        private void OnLocalNotificationTapped(NotificationTappedEventArgs e)
        {
            // your code goes here
        }
        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
