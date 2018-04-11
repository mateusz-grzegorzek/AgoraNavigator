using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using Plugin.Settings;
using System;
using System.Threading.Tasks;

namespace AgoraNavigator.Tasks
{
    class Beacons
    {
        static public string beaconIJHf = "IJHf";
        static public string beaconUQwa = "UQwa";
        static public string beaconFHNj = "FHNj";
        static public string beaconfvAL = "fvAL";
        static public string beaconGt6e = "Gt6e";
        static public string beaconJr2U = "Jr2U";
        static public string beaconYDt6 = "YDt6";
        static public string beaconKKoU = "KKoU";
        static public string beaconQ91y = "Q91y";
        static public string beaconnuyV = "nuyV";

        static IAdapter adapter = CrossBluetoothLE.Current.Adapter;
        static bool result;
        static string beaconNameToScan;
        static bool scanningForNewTasks;
        static int numberOfNewUnlockedTasks;

        public static bool IsBluetoothOn()
        {
            return CrossBluetoothLE.Current.IsOn;
        }

        public static void InitBeaconScanner()
        {
            adapter.DeviceDiscovered += OnDeviceDiscovered;
            adapter.ScanTimeout = 2000;
        }

        public static async Task<bool> ScanBeaconForNewTasks()
        {
            Console.WriteLine("ScanBeaconForNewTasks");
            result = false;           
            scanningForNewTasks = true;
            await adapter.StartScanningForDevicesAsync();
            return result;
        }

        public static async Task<bool> ScanForBeacon(string name)
        {
            Console.WriteLine("ScanForBeacon:beaconNameToScan=" + beaconNameToScan);
            beaconNameToScan = name;
            result = false;
            scanningForNewTasks = false;
            await adapter.StartScanningForDevicesAsync();
            return result;
        }

        public static async void OnDeviceDiscovered(object sender, DeviceEventArgs e)
        {
            IDevice dev = e.Device;
            if (scanningForNewTasks)
            {
                numberOfNewUnlockedTasks = CrossSettings.Current.GetValueOrDefault("Tasks:numberOfNewUnlockedTasks", 0);
                if (dev.Name == beaconIJHf)
                {
                    UnlockTasks(0);
                }
                else if (dev.Name == beaconUQwa)
                {
                    UnlockTasks(1);
                }
                else if (dev.Name == beaconFHNj)
                {
                    UnlockTasks(2);
                }
                else if (dev.Name == beaconfvAL)
                {
                    UnlockTasks(3);
                }
                else if (dev.Name == beaconGt6e)
                {
                    UnlockTasks(4);
                }
                else if (dev.Name == beaconJr2U)
                {
                    UnlockTasks(5);
                }
                else if (dev.Name == beaconYDt6)
                {
                    UnlockTasks(6);
                }
                else if (dev.Name == beaconKKoU)
                {
                    UnlockTasks(7);
                }
                else if (dev.Name == beaconQ91y)
                {
                    UnlockTasks(8);
                }
                else if (dev.Name == beaconnuyV)
                {
                    UnlockTasks(9);
                }
                CrossSettings.Current.AddOrUpdateValue("Tasks:numberOfNewUnlockedTasks", numberOfNewUnlockedTasks);
            }
            else
            {
                if (beaconNameToScan == dev.Name)
                {
                    Console.WriteLine("ScanForBeacon:dev.Id=" + dev.Id + ", dev.Name=" + dev.Name + ", dev.Rssi=" + dev.Rssi);
                    result = true;
                    await adapter.StopScanningForDevicesAsync();
                }
            }
        }

        private static void UnlockTasks(int taskSetId)
        {
            if (GameTask.UnlockTasks(numberOfNewUnlockedTasks, taskSetId))
            {
                numberOfNewUnlockedTasks += 3;
                result = true;
            }
        }
    }
}
