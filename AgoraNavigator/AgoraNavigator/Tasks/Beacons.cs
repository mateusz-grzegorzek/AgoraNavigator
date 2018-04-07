using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
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
        static IAdapter adapter;
        static bool result;
        static Guid guid;

        public static bool IsBluetoothOn()
        {
            return CrossBluetoothLE.Current.IsOn;
        }

        public static async Task<bool> ScanForBeacon(Guid _guid)
        {
            Console.WriteLine("ScanForBeacon:guid=" + guid);
            result = false;
            guid = _guid;
            adapter = CrossBluetoothLE.Current.Adapter;
            List<IDevice> deviceList = new List<IDevice>();
            adapter.DeviceDiscovered += OnDeviceDiscovered;
            await adapter.StartScanningForDevicesAsync(); 
            return result;
        }

        public static async void OnDeviceDiscovered(object sender, DeviceEventArgs e)
        {
            IDevice dev = e.Device;
            if (guid == dev.Id)
            {
                Console.WriteLine("ScanForBeacon:dev.Id=" + dev.Id + ", dev.Name=" + dev.Name + ", dev.Rssi=" + dev.Rssi);
                result = true;
                await adapter.StopScanningForDevicesAsync();
            }
            
        }
    }
}
