using System;
using System.Drawing;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace TravelAssistant.Custom
{
    public class NavigationCustomPage : Xamarin.Forms.NavigationPage
    {
        public static readonly BindableProperty LargeTitleProperty = BindableProperty.Create("Tint", typeof(bool), typeof(NavigationCustomPage), false);
        public NavigationCustomPage(Xamarin.Forms.Page page) : base(page)
        {
            On<iOS>().SetHideNavigationBarSeparator(true);
            On<iOS>().SetLargeTitleDisplay(LargeTitleDisplayMode.Never);
            Style = (Style)App.Current.Resources["NavigationStyle"];

            //App.Current.RequestedThemeChanged += (s, a) => this?.BackgroundColor=Color.AliceBlue;
            //BarBackgroundColor = Color.White;


        }
        public bool SetLargeTitleIos
        {
            get => On<iOS>().PrefersLargeTitles();
            set => On<iOS>().SetPrefersLargeTitles(value);
        }
        public static void PageLargeTitle(Xamarin.Forms.BindableObject page, bool large) {
            page.SetValue(LargeTitleProperty,large);
            //(page as NavigationCustomPage).On<iOS>().SetPrefersLargeTitles(large);
        }


    }
}
