using System;
using System.Collections.Generic;
using System.Text;

namespace AgoraNavigator
{
    public interface INotification
    {
        void Notify(string title, string msg);
    }
}
