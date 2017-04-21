using System;
using Microsoft.SPOT;
using System.Threading;
using GHIElectronics.NETMF.Net.Sockets;
using GHIElectronics.NETMF.Net;
namespace RoombaServer.Networking.RemoteCommands
{
    public class RemoteCommandReciever
    {
        private bool stop;
        public delegate void RemoteCommandRecievedDelegate(RemoteCommand remoteCommand);
        public event RemoteCommandRecievedDelegate RemoteCommandRecieved;

        private Thread workerThread;

        private const int MILLISECONDS_PER_SECOND = 1000;

        public RemoteCommandReciever()
        {
            stop = true;
        }
        public void Start()
        {
            if (stop)
            {
                stop = false;
                workerThread = new Thread(DoWork);
                workerThread.Start();
            }
        }
        private void DoWork()
        {
            Socket socket = new Socket(AddressFamily.InterNetwork,
                                         SocketType.Dgram, ProtocolType.Udp);
            IPEndPoint endPoint = new IPEndPoint(
                IPAddress.Any, 12344);

            EndPoint remoteEndPoint = new IPEndPoint(
                  IPAddress.Any, 0);
            socket.Bind(endPoint);
            byte[] commandBuffer = new byte[1 + (4 * 2)];
            int i = 0;

            while (!stop)
            {
                try
                {
                    if (socket.Poll(200 * MILLISECONDS_PER_SECOND, SelectMode.SelectRead))
                    {
                        Array.Clear(commandBuffer, 0, commandBuffer.Length);
                        socket.ReceiveFrom(commandBuffer, ref remoteEndPoint);
                        RemoteCommand command = GetRemoteCommand(commandBuffer);

                        if (RemoteCommandRecieved != null)
                        {
                            RemoteCommandRecieved(command);
                        }

                    }
                }
                catch (Exception e)
                {
                    Debug.Print("Error recieving command: " + e.Message);
                }
            }
        }
        public void Stop()
        {
            stop = true;
        }

        private RemoteCommand GetRemoteCommand(byte[] commandBuffer)
        {
            RemoteCommand command = new RemoteCommand();
            command.CommandType = (RemoteCommandType)commandBuffer[0];
            command.FirstParam = Common.GetIntFromByteArray(commandBuffer, 1);
            command.SecondParam = Common.GetIntFromByteArray(commandBuffer, 1 + 4);

            return command;

        }

    }

}
