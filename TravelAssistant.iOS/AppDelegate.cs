﻿using System;
using System.Collections.Generic;
using System.Linq;
using FFImageLoading.Forms.Platform;
using Foundation;
using UIKit;

namespace TravelAssistant.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            //Xamarin.Forms.Forms.SetFlags("AppTheme_Experimental");
            Xamarin.Forms.Forms.SetFlags("SwipeView_Experimental");
            Rg.Plugins.Popup.Popup.Init();
            Xamarin.FormsMaps.Init();
            
            

            Plugin.Segmented.Control.iOS.SegmentedControlRenderer.Initialize();

            FFImageLoading.Forms.Platform.CachedImageRenderer.Init();
            CachedImageRenderer.InitImageSourceHandler();
            Plugin.LocalNotification.NotificationCenter.AskPermission();
            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new App());
            return base.FinishedLaunching(app, options);
        }
        public override void WillEnterForeground(UIApplication uiApplication)
        {
            Plugin.LocalNotification.NotificationCenter.ResetApplicationIconBadgeNumber(uiApplication);
        }
    }
}
