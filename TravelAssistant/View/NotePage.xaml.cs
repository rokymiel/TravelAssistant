﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TravelAssistant.Models;
using Xamarin.Forms;

namespace TravelAssistant.View
{
    public partial class NotePage : ContentPage
    {
        private Note note;
        public NotePage()
        {
            InitializeComponent();
            note = new Note();
            BindingContext = this;

        }
        public NotePage(Note note)
        {
            InitializeComponent();
            BindingContext = this;
            this.note = note;

        }
        void OnNameChanged(object sender, TextChangedEventArgs e)
        {
            note.Name = Name.Text;
        }
        void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            note.Text = Text.Text;
        }
        protected override void OnAppearing()
        {
            if (note != null)
            {
                Name.Text = note.Name;
                Text.Text = note.Text;
            }
        }

        protected override void OnDisappearing()
        {
            if (note.Id == null || note.Id == string.Empty)
            {
                note.Id = Guid.NewGuid().ToString();
            }
            if (string.IsNullOrEmpty(note.Name) && string.IsNullOrEmpty(note.Text))
            {
                return;
            }
            if (string.IsNullOrEmpty(note.Name))
            {
                note.Name = "Без названия";
            }
            note.Date = DateTime.Now;
            
            NotesPage.Update(note);
            MessagingCenter.Send(this,"AddItem",note);
            
        }
    }
}
