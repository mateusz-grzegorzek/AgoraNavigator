using System;
using System.Collections.Generic;
using System.Text;

namespace AgoraNavigator.Login
{
    class User
    {
        public int Id { get; set; }
        public int Pin { get; set; }
    }

    class Users
    {
        public static List<User> users;

        static public void InitUsers()
        { 
            users = new List<User>();
            users.Add(new User { Id = 1, Pin = 1234 });
        }
    }
}
