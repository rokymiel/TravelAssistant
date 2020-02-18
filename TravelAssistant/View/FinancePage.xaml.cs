using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace TravelAssistant.View
{
    class Label
    {
        public string Text { get; set; }
    }
    public partial class FinancePage : ContentPage
    {
        ObservableCollection<Label> labels = new ObservableCollection<Label>();
        public FinancePage()
        {
            labels.Add(new Label { Text = "sdfsdfs" });
            labels.Add(new Label { Text = "sdfsdfs" });
            labels.Add(new Label { Text = "sdfsdfs" });
            labels.Add(new Label { Text = "sdfsdfs" });
            labels.Add(new Label { Text = "sdfsdfs" });
            labels.Add(new Label { Text = "sdfsdfs" });
            labels.Add(new Label { Text = "sdfsdfs" });
            labels.Add(new Label { Text = "sdfsdfs" });
            labels.Add(new Label { Text = "sdfsdfs" });

            InitializeComponent();
            finEvents.ItemsSource = labels;
        }
    }
}
