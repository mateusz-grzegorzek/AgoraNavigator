using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using AgoraNavigator.iOS;

[assembly: ExportRenderer(typeof(TabbedPage), typeof(ExtendedTabbedPageRenderer))]
namespace AgoraNavigator.iOS
{
    public class ExtendedTabbedPageRenderer : TabbedRenderer
    {
        public override void ViewWillLayoutSubviews()
        {
            var tabs = Element as TabbedPage;
            if (tabs != null && TabBar.Items != null)
            {
                for (int i = 0; i < TabBar.Items.Length; i++)
                {
                    UITabBarItem item = TabBar.Items[i];
                    if (item != TabBar.SelectedItem)
                    {
                        item.SetTitleTextAttributes(new UITextAttributes
                        {
                            Font = UIFont.FromName("ChalkboardSE-Light", 28.0F)
                        }, UIControlState.Normal);
                    }
                    else
                    {
                        item.SetTitleTextAttributes(new UITextAttributes
                        {
                            Font = UIFont.FromName("ChalkboardSE-Bold", 32.0F)
                        }, UIControlState.Normal);
                    }
                }
            }

            base.ViewWillLayoutSubviews();
        }

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            // Set Text Font for unselected tab states
            UITextAttributes normalTextAttributes = new UITextAttributes();
            normalTextAttributes.Font = UIFont.FromName("ChalkboardSE-Light", 28.0F); // unselected

            UITabBarItem.Appearance.SetTitleTextAttributes(normalTextAttributes, UIControlState.Normal);
        }

        public override UIViewController SelectedViewController
        {
            get
            {
                UITextAttributes selectedTextAttributes = new UITextAttributes();
                selectedTextAttributes.Font = UIFont.FromName("ChalkboardSE-Bold", 32.0F); // SELECTED
                if (base.SelectedViewController != null)
                {
                    base.SelectedViewController.TabBarItem.SetTitleTextAttributes(selectedTextAttributes, UIControlState.Normal);
                }
                return base.SelectedViewController;
            }
            set
            {
                base.SelectedViewController = value;

                foreach (UIViewController viewController in base.ViewControllers)
                {
                    UITextAttributes normalTextAttributes = new UITextAttributes();
                    normalTextAttributes.Font = UIFont.FromName("ChalkboardSE-Light", 28.0F); // unselected

                    viewController.TabBarItem.SetTitleTextAttributes(normalTextAttributes, UIControlState.Normal);
                }
            }
        }
    }
}
