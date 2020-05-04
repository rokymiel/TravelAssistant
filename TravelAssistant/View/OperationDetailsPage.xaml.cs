using System;
using System.Collections.Generic;
using TravelAssistant.Models;
using Xamarin.Forms;

namespace TravelAssistant.View
{
    public partial class OperationDetailsPage : Rg.Plugins.Popup.Pages.PopupPage
    {
        /// <summary>
        /// Выбранная операция.
        /// </summary>
        MoneyOperation Money { get; set; }
        public OperationDetailsPage(MoneyOperation money)
        {
            Money = money;
            InitializeComponent();
            Animation = new Rg.Plugins.Popup.Animations.MoveAnimation();
            cakeView.BindingContext = Money;
        }
    }
}
