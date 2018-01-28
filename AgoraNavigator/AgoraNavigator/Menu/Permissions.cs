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
            Console.WriteLine("OnMyLocationButtonClicked");
            bool permissionsGranted = false;
            try
            {
                
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(permission);
                if (status != PermissionStatus.Granted)
                {
                    Console.WriteLine("Not granted yet!");
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(permission))
                    {
                        //await DisplayAlert("Need location", "OK", "Cancel");
                    }
                    var results = await CrossPermissions.Current.RequestPermissionsAsync(permission);
                    status = results[Permission.Location];
                    if (status == PermissionStatus.Granted)
                    {
                        Console.WriteLine("Access granted! :)");
                        permissionsGranted = true;
                    }
                    else
                    {
                        Console.WriteLine("Not granted! :(");
                    }
                }
                else
                {
                    Console.WriteLine("Already granted!");
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
