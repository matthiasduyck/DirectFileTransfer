using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.WiFiDirect;
using Windows.Foundation;

namespace Direct_File_Transfer
{
    class WifiDirectManager
    {
        Windows.Devices.WiFiDirect.WiFiDirectDevice wfdDevice;
        async internal void testconnectAsync()
        {
            string result = "";
            string deviceSelector = WiFiDirectDevice.GetDeviceSelector(WiFiDirectDeviceSelectorType.AssociationEndpoint);
            DeviceInformationCollection devInfoCollection = await DeviceInformation.FindAllAsync(deviceSelector);

            String deviceId = devInfoCollection[0].Id;
            wfdDevice = await Windows.Devices.WiFiDirect.WiFiDirectDevice.FromIdAsync(deviceId);

            if (wfdDevice == null)
            {
                result = "Connection to " + deviceId + " failed.";
            }

            // Register for connection status change notification.
            wfdDevice.ConnectionStatusChanged += new TypedEventHandler<Windows.Devices.WiFiDirect.WiFiDirectDevice, object>(OnConnectionChanged);

            // Get the EndpointPair information.
            var EndpointPairCollection = wfdDevice.GetConnectionEndpointPairs();

            if (EndpointPairCollection.Count > 0)
            {
                var endpointPair = EndpointPairCollection[0];
                result = "Local IP address " + endpointPair.LocalHostName.ToString() +
                    " connected to remote IP address " + endpointPair.RemoteHostName.ToString();
            }
            else
            {
                result = "Connection to " + deviceId + " failed.";
            }
        }

        private void OnConnectionChanged(object sender, object arg)
        {
            Windows.Devices.WiFiDirect.WiFiDirectConnectionStatus status =
                (Windows.Devices.WiFiDirect.WiFiDirectConnectionStatus)arg;

            if (status == Windows.Devices.WiFiDirect.WiFiDirectConnectionStatus.Connected)
            {
                // Connection successful.
            }
            else
            {
                // Disconnected.
                Disconnect();
            }
        }

        private void Disconnect()
        {
            if (wfdDevice != null)
            {
                wfdDevice.Dispose();
            }
        }
    }
}
