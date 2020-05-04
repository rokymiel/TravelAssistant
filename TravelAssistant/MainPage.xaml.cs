using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAssistant.Models;
using Xamarin.Forms;

namespace TravelAssistant
{

    public partial class MainPage : TabbedPage
    {
        public static Trip CurrentTrip { get; private set; }
        public MainPage(Trip trip)
        {
            CurrentTrip = trip;
            
            InitializeComponent();
        }
    }

}

