using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;

namespace BeerBong.Services
{
     
    
    class BlueToothService
    {
        private IAdapter adapter;
        private IBluetoothLE ble;
        ObservableCollection<IDevice> deviceList;

        public async Task BlueToothHandler()
        {
             ble = CrossBluetoothLE.Current;
             adapter = CrossBluetoothLE.Current.Adapter;

            
            deviceList = new ObservableCollection<IDevice>();

            
        }

        private async void btnclick(object sender, EventArgs e)
        {
            adapter.DeviceDiscovered += (s, a) => { deviceList.Add(a.Device); };

            await adapter.StartScanningForDevicesAsync();
        }

    }
}
