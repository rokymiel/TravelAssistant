using System;
using Plugin.LocalNotification;
using TravelAssistant.Custom;
using TravelAssistant.Managers;
using TravelAssistant.Models;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;

namespace TravelAssistant
{
    public partial class App : Xamarin.Forms.Application
    {
        string dbNotesPath = FileAccessManager.GetLocalFilePath("notes.db3");
        string dbRemindersPath = FileAccessManager.GetLocalFilePath("reminders.db3");
        string dbMoneyPath = FileAccessManager.GetLocalFilePath("money.db3");
        string dbMoneyOperationsPath = FileAccessManager.GetLocalFilePath("moneyOperations.db3");
        string dbDocumentsPath = FileAccessManager.GetLocalFilePath("documents.db3");
        string dbPlacesPath = FileAccessManager.GetLocalFilePath("placess.db3");
        string dbTripsPath = FileAccessManager.GetLocalFilePath("trips.db3");

        public static SQLManager<Note> notesManger;
        public static SQLManager<Reminder> remindersManger;
        public static SQLManager<Money> moneyManager;
        public static SQLManager<MoneyOperation> moneyOperationManager;
        public static SQLManager<Document> documentManager;
        public static SQLManager<Place> placesManager;
        public static SQLManager<Trip> tripsManager;

        public App()
        {
            Device.SetFlags(new string[] { "AppTheme_Experimental", "SwipeView_Experimental" });
            InitializeComponent();
            notesManger = new SQLManager<Note>(dbNotesPath);
            remindersManger = new SQLManager<Reminder>(dbRemindersPath);
            moneyManager = new SQLManager<Money>(dbMoneyPath);
            moneyOperationManager = new SQLManager<MoneyOperation>(dbMoneyOperationsPath);
            documentManager = new SQLManager<Document>(dbDocumentsPath);
            placesManager = new SQLManager<Place>(dbPlacesPath);
            tripsManager = new SQLManager<Trip>(dbTripsPath);
            NotificationCenter.Current.NotificationTapped += OnLocalNotificationTapped;
            
            if (Properties.ContainsKey("mainPage"))
            {
                //var navigationPage = new Xamarin.Forms.NavigationPage(new MainPage(tripsManager.GetTripById(Properties["mainPage"].ToString())));
                //navigationPage.On<iOS>().SetPrefersLargeTitles(true);
                MainPage = new MainPage(tripsManager.GetTripById(Properties["mainPage"].ToString()));
            }
            else
            {
                var navi = new NavigationCustomPage(new AllTripsPage()) { SetLargeTitleIos = true };
                MainPage = navi;
            }

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
