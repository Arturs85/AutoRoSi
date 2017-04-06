using System;
using Microsoft.SPOT;
using System.Threading;
using GHIElectronics.NETMF.Net;
using GHIElectronics.NETMF.Net.Sockets;
using System.Text;
using RoombaServer.Roomba.Sensors;

namespace RoombaServer.Networking.StatusSending
{
    public class RoombaStatusSender
    {
        private SensorController sensors;
        private Thread workingThread;

        public RoombaStatusSender(SensorController sensors)
        {

            this.sensors = sensors;
        }
        public void Start()
        {
            workingThread = new Thread(DoWork);
            workingThread.Start();
        }
        private void DoWork()
        {

            Socket socket = new Socket(AddressFamily.InterNetwork,
                               SocketType.Dgram, ProtocolType.Udp);
            IPEndPoint routerEndPoint = new IPEndPoint(
             new IPAddress(new byte[] { 192, 168, 17, 1 }), // ?? ???????
                                   80); // http ????
            IPEndPoint endPoint = new IPEndPoint(
                  new IPAddress(new byte[] { 192, 168, 17, 141 }), // ?? ???????
                                        666); 
            byte[] data = new byte[(4 * 5) + 1];
            int i = 0;

            while (true)
            {
                Array.Clear(data, 0, data.Length);
                Common.GetByteArrayFromInt((int)sensors.BatteryPercentage, data, 16);
                data[20] = sensors.BumpsWheeldropsData;
                socket.SendTo(data, endPoint);
                if (i++ % 100 == 0)
                {
                    socket.SendTo(new byte[] { 1 }, routerEndPoint);
                }
                Thread.Sleep(50);
            }
        }
    }
}

