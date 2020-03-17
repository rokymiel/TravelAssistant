using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TravelAssistant.Models;
using System.Linq;
using Xamarin.Forms;

namespace TravelAssistant.View
{
    public partial class NotesPage : ContentPage
    {
        static ObservableCollection<Note> items { get; set; }
        public NotesPage()
        {
            InitializeComponent();
            items = new ObservableCollection<Note>();
            App.notesManger.GetNotes().ForEach(x=>items.Add(x));
            items = new ObservableCollection<Note>(items.OrderByDescending(x => x.Date));
            listView.ItemsSource = items;


        }
        async void CreateNewNote(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NotePage());
        }
        async void OnSelectedNote(object sender, SelectedItemChangedEventArgs args)         {
            
            var item = args.SelectedItem as Note;             if (item == null)                 return;                          await Navigation.PushAsync(new NotePage(item));              // Manually deselect item.             listView.SelectedItem = null;         }


        public static void Update(Note item)
        {

            if (item != null)
            {
                int index = FindInCollection(items, item);
                if (index >= 0)
                {
                    items[index] = item;
                    App.notesManger.Update(item);
                }
                else
                {
                    items?.Add(item);
                    App.notesManger.AddItem(item);
                }
                // Убрать сортировку при неизменении заметок.
                // Убрать Выделение после выбора.
                items = new ObservableCollection<Note>(items.OrderByDescending(x => x.Date));
                
            }

        }
        public static int FindInCollection(ObservableCollection<Note> ts, Note item)
        {
            for (int i = 0; i < ts.Count; i++)
            {
                if (ts[i].Id == item.Id)
                {
                    return i;
                }
            }
            return -1;
        }
        void OnDelete(object sender, EventArgs e)
        {
            
            var i = ((MenuItem)sender).CommandParameter as Note;
            
            items.Remove(i);
            App.notesManger.DeleteItem(i);
        }
        protected override void OnAppearing()
        {
            if (items != null && items.Count > 0)
            {
                //listView.ItemsSource = items;
                listView.IsVisible = true;
                //noNotesLable.IsVisible = false;

            }
            else
            {
                //listView.IsVisible = false;
                //noNotesLable.IsVisible = true;
            }
            listView.ItemsSource = items;
        }
    }
}
