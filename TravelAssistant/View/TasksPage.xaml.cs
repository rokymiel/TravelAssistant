using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace TravelAssistant.View
{
    public partial class TasksPage : ContentPage
    {
        public TasksPage()
        {
            InitializeComponent();
        }

        async void OnNotesClicked(System.Object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new NotesPage());
        }
        async void OnRemindersClicked(System.Object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new RemindersPage());
        }

        

    }
}
