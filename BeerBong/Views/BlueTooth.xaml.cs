using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.BLE;
using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.Exceptions;


namespace BeerBong.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BlueTooth : ContentPage
    {
        private IBluetoothLE ble;
        private IAdapter adapter;
        private ObservableCollection<IDevice> deviceList;
        private IDevice device;
        private IList<ICharacteristic> characteristics;
        private ICharacteristic Characteristic;
        IReadOnlyList<IService> Services;
         IService Service;
        private IDescriptor Descriptor;
        private Guid id;

        public BlueTooth()
        {
            
            InitializeComponent();
            ble = CrossBluetoothLE.Current;
            adapter = CrossBluetoothLE.Current.Adapter;
            deviceList = new ObservableCollection<IDevice>();
            

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            DeviceList.ItemsSource = deviceList;
        }


        private async void btnStatus_Clicked(object sender, EventArgs e)
        {
            var status = ble.State;
            await DisplayAlert("BT Status", status.ToString(), "OK");
        }

        async void btScan_Clicked(object sender, EventArgs e)
        {
            
            adapter.ScanMode = ScanMode.Balanced;
            adapter.DeviceDiscovered += (s, a) =>
            {
                if (a.Device.Name != null)
                {
                    deviceList.Add(a.Device);
                    
                }
                if (a.Device.NativeDevice.ToString() == "B8:27:EB:D5:09:AF")
                {
                    DisplayAlert("Device fundet", "Device ID:" + a.Device.Name, "OK");
                    deviceList.Add(a.Device);
                     id = a.Device.Id;
                }
                string test = a.Device.NativeDevice.ToString();
                tester.Text = test;

            };

            if (!ble.Adapter.IsScanning)
            {
                
                await adapter.StartScanningForDevicesAsync();
            }
        }


        async void btConnect_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (device != null)
                {
                    await adapter.ConnectToDeviceAsync(device);
                }
                else
                {
                    await DisplayAlert("Problem", "Intet device valgt!", "OK");
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }

        }

        private async void btnKnowConnect_clicked(object sender, EventArgs e)
        {
            try
            {
                await adapter.ConnectToKnownDeviceAsync(new Guid("guid"));
            }
            catch (DeviceConnectionException ex)
            {
                await DisplayAlert("Problem", ex.Message.ToString(), "OK");
            }
        }

        private void dv_Items(object sender, SelectedItemChangedEventArgs e)
        {
            
            if (DeviceList.SelectedItem == null)
            {
                return;
            }
            device = DeviceList.SelectedItem as IDevice;
            
        }
        private async void btnReadChar(object sender, EventArgs e)
        {
            var bytes = await Characteristic.ReadAsync();
            await Characteristic.WriteAsync(bytes);
        }
        private async void btnGetServices_Clicked(object sender, EventArgs e)
        {
            Services = await device.GetServicesAsync();
            Service = await device.GetServiceAsync(Guid.Parse("00001801-0000-1000-8000-00805f9b34fb"));
        }

        private async void btnGetChar(object sender, EventArgs e)
        {
            var characteristics = await Service.GetCharacteristicsAsync();
            Guid idGuid = Guid.Parse("guid");
            Characteristic = await Service.GetCharacteristicAsync(Guid.Parse("00001801-0000-1000-8000-00805f9b34fb"));


        }

        private async void btn_GetDescriptor()
        {
            Descriptor = await Characteristic.GetDescriptorAsync(id);
        }

        private async void btn_ReadDescripter()
        {
            var bytes = await Descriptor.ReadAsync();
            await Descriptor.WriteAsync(bytes);
        }

       


    }


}