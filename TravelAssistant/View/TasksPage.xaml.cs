using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using TravelAssistant.Models;
using TravelAssistant.Services;
using Xamarin.Forms;

namespace TravelAssistant.View
{
    public partial class TasksPage : ContentPage
    {
        public TasksPage()
        {
            InitializeComponent();
            BindingContext = VM;
            VM.GetImage += OnImageGetted;
            documents = new ObservableCollection<Document>();
            
            //var s=App.documentManager.GetDocuments();
            //s.ForEach(x=> x.Image=ImageSource.FromFile(x.Path));
            //s.ForEach(x=>documents.Add(x));
            col.ItemsSource = documents;
            
        }
        protected override void OnAppearing()
        {
            int notesCount = App.notesManger.GetNotes().Count;
            int remindersCount = App.remindersManger.GetReminder().Count;
            notesCount = notesCount <= 0 ? 0 : notesCount;
            remindersCount = remindersCount <= 0 ? 0 : remindersCount;
            notesCountLabel.Text = $"Всего: {notesCount}";
            remindersCountLabel.Text = $"Всего: {remindersCount}";
        }
        private static PhotoModel VM = new PhotoModel();
        static ObservableCollection<Document> documents;
        async void OnNotesClicked(System.Object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new NotesPage());
        }
        async void OnRemindersClicked(System.Object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new RemindersPage());
        }
        async void OnDocumentSelectionChanged(System.Object sender, Xamarin.Forms.SelectionChangedEventArgs e)
        {
            var item = e.CurrentSelection.FirstOrDefault() as Document;                          if (item == null)
            {


                col.SelectedItem = null;
                return;
            }
            await Navigation.PushModalAsync(new ImagePage(item.Image));
            (sender as CollectionView).SelectedItem = null;

        }
        private static int c = 0;
        public void OnImageGetted(object sender, ImageEventArgs e)
        {
            //(sender as Button).IsEnabled = false;

            
            Document document = new Document();
            document.Id= Guid.NewGuid().ToString();
            document.Image = e.Image;
            document.Path = e.Path;
            documents.Add(document);
            //App.documentManager.AddItem(document);
            //App.documentManager.AddItem(document);
            //(sender as Button).IsEnabled = true;
        }
        async void OnPickPhotoButtonClicked(object sender, EventArgs e)
        {
            (sender as Button).IsEnabled = false;

            //Stream stream = await DependencyService.Get<IPhotoPickerService>().GetImageStreamAsync();
            //if (stream != null)
            //{
            //    Document document = new Document();

            //    document.Image = ImageSource.FromStream(() => stream);
            //    documents.Add(document);
            //    //image.Source = document.Image;
            //}
            Document document = new Document();
            VM.GetImageAndRun();
            document.Image = VM.ImageSource;
            documents.Add(document);
            

            (sender as Button).IsEnabled = true;
        }


    }
}
