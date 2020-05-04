using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FFImageLoading.Forms;
using Plugin.Media;
using TravelAssistant.Managers;
using TravelAssistant.Models;
using Xamarin.Forms;
using Xamarin.Forms.PancakeView;

namespace TravelAssistant.View
{
    public partial class TasksPage : ContentPage
    {
        /// <summary>
        /// Объект для работы с изображениями.
        /// </summary>
        private static PhotoModel VM = new PhotoModel();
        /// <summary>
        /// Список документов.
        /// </summary>
        static ObservableCollection<Document> documents;

        public TasksPage()
        {
            InitializeComponent();
            BindingContext = VM;
            VM.GetImage += OnImageGetted;
            documents = new ObservableCollection<Document>();
            var s = App.documentManager.GetTripItems<Document>(MainPage.CurrentTrip.Id);
            s.ForEach(x => documents.Add(x));
            col.ItemsSource = documents;

        }

        protected override void OnAppearing()
        {
            int notesCount = App.notesManger.GetTripItems<Note>(MainPage.CurrentTrip.Id).Count;
            int remindersCount = App.remindersManger.GetTripItems<Reminder>(MainPage.CurrentTrip.Id).Count;
            notesCount = notesCount <= 0 ? 0 : notesCount;
            remindersCount = remindersCount <= 0 ? 0 : remindersCount;
            notesCountLabel.Text = $"Всего: {notesCount}";
            remindersCountLabel.Text = $"Всего: {remindersCount}";
        }
        
        async void OnNotesClicked(System.Object sender, System.EventArgs e)
        {
            await AnimationManager.StartScalePancakeView(sender);
            await Navigation.PushAsync(new NotesPage());
        }

        async void OnRemindersClicked(System.Object sender, System.EventArgs e)
        {
            await AnimationManager.StartScalePancakeView(sender);
            await Navigation.PushAsync(new RemindersPage());
        }

        async void DeleteImage_Invoked(System.Object sender, System.EventArgs e)
        {
            if (await DisplayAlert("Удалить фотографию?", "Фотография будет удалена, данное действие необратимо.", "Удалить", "Отмена"))
            {
                var document = (Document)(sender as SwipeItemView).CommandParameter;
                documents.Remove(document);
                App.documentManager.DeleteItem(document);
            }
        }

        async void Image_Tapped(System.Object sender, System.EventArgs e)
        {
            var item = (Document)(sender as CachedImage).BindingContext;

            if (item == null)
            {
                return;
            }
            await Navigation.PushModalAsync(new ImagePage(item.Image));
        }

        async void OnDocumentSelectionChanged(System.Object sender, Xamarin.Forms.SelectionChangedEventArgs e)
        {
            var item = e.CurrentSelection.FirstOrDefault() as Document;

            if (item == null)
            {
                col.SelectedItem = null;
                return;
            }
            await Navigation.PushModalAsync(new ImagePage(item.Image));
            (sender as CollectionView).SelectedItem = null;

        }

        public void OnImageGetted(object sender, ImageEventArgs e)
        {
            Document document = new Document();
            document.Id = Guid.NewGuid().ToString();
            document.ByteImage = e.ByteImage;
            document.Path = e.Path;
            document.TripId = MainPage.CurrentTrip.Id;
            documents.Add(document);
            App.documentManager.AddItem(document);
        }

    }
}
