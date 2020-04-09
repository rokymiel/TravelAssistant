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
using TravelAssistant.Services;
using Xamarin.Forms;
using Xamarin.Forms.PancakeView;

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
            
            var s=App.documentManager.GetDocuments();
            //s.ForEach(x=> x.Image=ImageSource.FromFile(x.Path));
            s.ForEach(x=>documents.Add(x));
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
        private async void StartAnimation(PancakeView pancake)
        {
            await pancake.FadeTo(0.4, 200);
            await Task.Delay(100);
            await pancake.FadeTo(1, 200);
        }
        private static PhotoModel VM = new PhotoModel();
        static ObservableCollection<Document> documents;
        async void OnNotesClicked(System.Object sender, System.EventArgs e)
        {
            // ANIMATION SCALE.
            //var s = (sender as PancakeView);
            //await s.ScaleTo(0.9, 100);
            //await s.ScaleTo(1, 100);
            await AnimationManager.StartScalePancakeView(sender);
            //StartAnimation(notesCardView);
            await Navigation.PushAsync(new NotesPage());
        }
        async void OnRemindersClicked(System.Object sender, System.EventArgs e)
        {
            // ANIMATION SCALE.
            //var s = (sender as PancakeView);
            //await s.ScaleTo(0.9, 100);
            //await s.ScaleTo(1, 100);
            await AnimationManager.StartScalePancakeView(sender);
            //StartAnimation(remindersCardView);
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
            Console.WriteLine();
            Console.WriteLine((e as TappedEventArgs));
            var item = (Document)(sender as CachedImage).BindingContext;

            if (item == null)
            {
                return;
            }
            await Navigation.PushModalAsync(new ImagePage(item.Image));
        }
        async void OnDocumentSelectionChanged(System.Object sender, Xamarin.Forms.SelectionChangedEventArgs e)
        {
            Console.WriteLine("22222222222");
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
            //document.Image = e.Image;
            document.ByteImage = e.ByteImage;
            //document.Image = ;
            document.Path = e.Path;
            documents.Add(document);
            App.documentManager.AddItem(document);
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
            //document.Image = VM.ImageSource;
            documents.Add(document);
            

            (sender as Button).IsEnabled = true;
        }

        
        //SelectionChanged="OnDocumentSelectionChanged"

    }
}
