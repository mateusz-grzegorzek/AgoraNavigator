using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AgoraNavigator.Tasks
{
    class Beacons
    {
        static public Guid beaconFHNJ = new Guid("00000000-0000-0000-0000-e69696ad6edd");
        static public Guid beaconUQwa = new Guid("00000000-0000-0000-0000-e372a211aec2");
        static public Guid beaconIJHf = new Guid("00000000-0000-0000-0000-d3ca630b17c0");

        static public async Task<bool> ScanForBeacon(Guid guid)
        {
            bool result = false;
            var adapter = CrossBluetoothLE.Current.Adapter;
            adapter.ScanTimeout = 2000;
            List<IDevice> deviceList = new List<IDevice>();
            adapter.DeviceDiscovered += (s, a) => deviceList.Add(a.Device);
            await adapter.StartScanningForDevicesAsync();
            Console.WriteLine("ScanForBeaconAsync:guid=" + guid);
            foreach (IDevice dev in deviceList)
            {
                Console.WriteLine("ScanForBeaconAsync:dev.Id=" + dev.Id + ", dev.Name=" + dev.Name + ", dev.Rssi=" + dev.Rssi);
                if(guid == dev.Id)
                {
                    result = true;
                }
            }
            return result;
        }
    }
}
