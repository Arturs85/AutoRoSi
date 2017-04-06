using System;
using Microsoft.SPOT;

using Microsoft.SPOT.Hardware;
using GHIElectronics.NETMF.Net.NetworkInformation;
using GHIElectronics.NETMF.FEZ;
using GHIElectronics.NETMF.Net;

namespace RoombaServer.Networking
{
  public  class NetworkManager
    {

        private void InitNetworking()
        {
            WIZnet_W5100.Enable(SPI.SPI_module.SPI1, (Cpu.Pin)FEZ_Pin.Digital.Di10, (Cpu.Pin)FEZ_Pin.Digital.Di7, false);

            byte[] ipAddress = new byte[] { 192, 168, 17, 41 };
            byte[] netmask = new byte[] { 255, 255, 255, 0 };
            byte[] gateway = new byte[] { 192, 168, 17, 1 };
            // set our own mac address for this device
            byte[] macAddress = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x41 };

            NetworkInterface.EnableStaticIP(ipAddress, netmask, gateway, macAddress);

        }
 
}
}
