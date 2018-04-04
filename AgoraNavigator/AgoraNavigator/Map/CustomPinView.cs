using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AgoraNavigator.GoogleMap
{
    class CustomPinView : ContentView
    {
        private Image bgImage;
        private Label icon;

        public CustomPinView(string bgImageSource, string iconChar)
        {
            WidthRequest = 60;
            HeightRequest = 67;
            AnchorX = 0.5;
            AnchorY = 1;

            icon = new Label()
            {
                Text = iconChar,
                TextColor = Color.White,
                FontSize = 28,
                Opacity = 0.5,
                FontFamily = AgoraFonts.GetFontAwesome(),
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center
            };

            bgImage = new Image()
            {
                Source = bgImageSource,
                Aspect = Aspect.AspectFill
            };

            var layout = new RelativeLayout()
            {
                WidthRequest = 60,
                HeightRequest = 67
            };

            layout.Children.Add(bgImage,
                Constraint.Constant(0),
                Constraint.Constant(0),
                Constraint.RelativeToParent((parent) => { return 60; }),
                Constraint.RelativeToParent((parent) => { return 67; }));

            layout.Children.Add(icon,
                Constraint.Constant(0),
                Constraint.Constant(0),
                Constraint.RelativeToParent((parent) => { return 60; }),
                Constraint.RelativeToParent((parent) => { return 67; }));

            Content = layout;
        }
    }
}
