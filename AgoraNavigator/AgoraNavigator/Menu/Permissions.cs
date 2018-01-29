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
                    Console.WriteLine("Permissions:GetRuntimePermissionNot granted yet!");
                    var results = await CrossPermissions.Current.RequestPermissionsAsync(permission);
                    status = results[Permission.Location];
                    if (status == PermissionStatus.Granted)
                    {
                        Console.WriteLine("Permissions:GetRuntimePermissionAccess granted! :)");
                        permissionsGranted = true;
                    }
                    else
                    {
                        Console.WriteLine("Permissions:GetRuntimePermissionNot granted! :(");
                    }
                }
                else
                {
                    Console.WriteLine("Permissions:GetRuntimePermissionAlready granted!");
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
