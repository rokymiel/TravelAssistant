using System;
using TravelAssistant.Custom;
using TravelAssistant.iOS.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(NavigationCustomPage), typeof(NavigationCustomRenderer))]

namespace TravelAssistant.iOS.Renderers
{
    public class NavigationCustomRenderer: NavigationRenderer
    {
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            if (Element is Xamarin.Forms.NavigationPage navigationPage)
            {
                //iOS older version fix
                NavigationBar.SetBackgroundImage(new UIKit.UIImage(), UIKit.UIBarMetrics.Default);
                NavigationBar.ShadowImage = new UIKit.UIImage();
                NavigationBar.ClipsToBounds = false;
                try //Newest iOS version fix - trycatch isn't optimal
                {
                    NavigationBar.ScrollEdgeAppearance.ShadowImage = new UIKit.UIImage();
                    NavigationBar.ScrollEdgeAppearance.ShadowColor = null;
                }
                catch (Exception) { }
            }
        }
    }
}
