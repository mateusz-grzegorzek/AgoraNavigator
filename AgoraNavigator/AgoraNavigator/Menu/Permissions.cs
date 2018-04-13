using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Threading.Tasks;

namespace AgoraNavigator.Menu
{
    class Permissions
    {
        public static async Task<bool> GetRuntimePermission(Permission permission)
        {
            Console.WriteLine("Permissions:GetRuntimePermission");
            bool permissionsGranted = false;
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(permission);
                if (status != PermissionStatus.Granted)
                {
                    Console.WriteLine("Permissions:GetRuntimePermission: Not granted yet!");
                    var results = await CrossPermissions.Current.RequestPermissionsAsync(permission);
                    status = results[permission];
                    if (status == PermissionStatus.Granted)
                    {
                        Console.WriteLine("Permissions:GetRuntimePermission: Access granted! :)");
                        permissionsGranted = true;
                    }
                    else
                    {
                        Console.WriteLine("Permissions:GetRuntimePermission: Not granted! :(");
                    }
                }
                else
                {
                    Console.WriteLine("Permissions:GetRuntimePermission: Already granted!");
                    permissionsGranted = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return permissionsGranted;
        }
    }
}
