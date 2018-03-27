using Android.App;
using Android.Support.V4.App;
using Android.Content;

[assembly: Xamarin.Forms.Dependency(typeof(AgoraNavigator.Droid.Notification_Android))]
namespace AgoraNavigator.Droid
{
    public class Notification_Android : INotification
    {
        Context context;
        NotificationCompat.Builder builder;
        NotificationManager manager;

        private int notificationId = 0;

        public Notification_Android()
        {
            context = Application.Context;

            builder = new NotificationCompat.Builder(context)
                .SetAutoCancel(true)                            // Dismiss from the notif. area when clicked
                .SetContentTitle("")                            // Set its title
                .SetContentText("")                             // The message to display
                .SetSmallIcon(Resource.Drawable.Hamburger_Icon);     // Set icon

            manager = (NotificationManager)context.GetSystemService(Context.NotificationService);

        }

        public void Notify(string title, string msg)
        {
            builder.SetContentTitle(title);
            builder.SetContentText(msg);

            Notification notification = builder.Build();

            manager.Notify(notificationId++, notification);
        }
    }
}