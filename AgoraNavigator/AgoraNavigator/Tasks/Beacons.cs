using AgoraNavigator.Login;
using AgoraNavigator.Popup;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using Plugin.Settings;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AgoraNavigator.Tasks
{
    class Beacons
    {
        static public Guid beaconIJHf = new Guid("00000000-0000-0000-0000-e69696ad6edd");
        static public Guid beaconUQwa = new Guid("00000000-0000-0000-0000-e372a211aec2");
        static public Guid beaconFHNJ = new Guid("00000000-0000-0000-0000-d3ca630b17c0");
        static IAdapter adapter = CrossBluetoothLE.Current.Adapter;
        static bool result;
        static Guid guid;
        static bool scanningForNewTasks;

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

        public static async Task<bool> ScanForBeacon(Guid _guid)
        {
            Console.WriteLine("ScanForBeacon:guid=" + guid);
            guid = _guid;
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
                int numberOfNewUnlockedTasks = CrossSettings.Current.GetValueOrDefault("Tasks:numberOfNewUnlockedTasks", 0);
                if (dev.Id == beaconIJHf)
                {
                    if(GameTask.UnlockTasks(numberOfNewUnlockedTasks, 0))
                    {
                        numberOfNewUnlockedTasks += 3;
                        result = true;
                    }
                }
                else if (dev.Id == beaconUQwa)
                {
                    if (GameTask.UnlockTasks(numberOfNewUnlockedTasks, 1))
                    {
                        numberOfNewUnlockedTasks += 3;
                        result = true;
                    }
                }
                else if (dev.Id == beaconFHNJ)
                {
                    if (GameTask.UnlockTasks(numberOfNewUnlockedTasks, 2))
                    {
                        numberOfNewUnlockedTasks += 3;
                        result = true;
                    }
                }
                CrossSettings.Current.AddOrUpdateValue("Tasks:numberOfNewUnlockedTasks", numberOfNewUnlockedTasks);
            }
            else
            {
                if (guid == dev.Id)
                {
                    Console.WriteLine("ScanForBeacon:dev.Id=" + dev.Id + ", dev.Name=" + dev.Name + ", dev.Rssi=" + dev.Rssi);
                    result = true;
                    await adapter.StopScanningForDevicesAsync();
                }
            }
        }
    }
}
