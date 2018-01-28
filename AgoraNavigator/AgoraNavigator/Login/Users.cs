using AgoraNavigator.Tasks;
using Newtonsoft.Json;
using PCLStorage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AgoraNavigator.Login
{
    class User
    {
        public int Id { get; set; }
        public int Pin { get; set; }
        public int AntentaId { get; set; }
        public int TotalPoints { get; set; }
    }

    class Users
    {
        public static Dictionary<int, List<int>> AntenaMembers;
        private static IFolder rootFolder;
        private static IFolder userDataFolder;
        private static IFile userDataFile;

        public static List<User> users;
        public static User loggedUser;

        public static void InitUsers()
        {
            users = new List<User>();
            users.Add(new User { Id = 1, Pin = 1234 });
            AntenaMembers.Add(1, new List<int> { 2 });
        }

        public async static void InitUserData(User user)
        {
            loggedUser = user;
            rootFolder = FileSystem.Current.LocalStorage;
            userDataFolder = await rootFolder.CreateFolderAsync("user_data_" + user.Id, CreationCollisionOption.OpenIfExists);
            try
            {
                userDataFile = await userDataFolder.CreateFileAsync("user_data.txt", CreationCollisionOption.FailIfExists);
                loggedUser.TotalPoints = 0;
                String userData = JsonConvert.SerializeObject(loggedUser);
                await userDataFile.WriteAllTextAsync(userData);
            }
            catch (Exception)
            {
                userDataFile = await userDataFolder.CreateFileAsync("user_data.txt", CreationCollisionOption.OpenIfExists);
                String userData = await userDataFile.ReadAllTextAsync();
                loggedUser = JsonConvert.DeserializeObject<User>(userData);
                Console.WriteLine("loggedUser.TotalPoints=" + loggedUser.TotalPoints);
            }
        }

        public async static Task addScorePoints(int scorePoints)
        {
            String userData = await userDataFile.ReadAllTextAsync();
            loggedUser = JsonConvert.DeserializeObject<User>(userData);
            loggedUser.TotalPoints += scorePoints;
            Console.WriteLine("loggedUser.TotalPoints=" + loggedUser.TotalPoints);
            userData = JsonConvert.SerializeObject(loggedUser);
            await userDataFile.WriteAllTextAsync(userData);
        }
    }
}
