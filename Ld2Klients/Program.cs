using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Program p = new Program();
            p.RunClient();
        }

        private void RunClient()
        {
            IPEndPoint serverEndpoint = new IPEndPoint(
                new IPAddress(new byte[] { 192, 168, 17, 41 }), // ierices (servera) adrese
                12345); // porta numurs

            while (true)
            {
                Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                try
                {
                    client.Connect(serverEndpoint);

                    SendRequestToServer(client);
                    ReadServerResponse(client);
                }
                catch (Exception e)
                {
                    Debug.Print("Failed to connect to server: " + e.Message);
                }
                finally
                {
                    client.Close();
                    Thread.Sleep(1000);
                }
            }
        }

        private void ReadServerResponse(Socket client)
        {
            const int microSecondsPerSecond = 1000000;

            // gaidit dauts no servera 10 sekundes
            if (client.Poll(10 * microSecondsPerSecond, SelectMode.SelectRead) && client.Available > 0)
            {
                byte[] response = new byte[client.Available];
                client.Receive(response,response.Length,SocketFlags.None);
                String responseString = new String(Encoding.UTF8.GetChars(response));
                Debug.Print("Atbilde no servera: "+responseString);
            }
        }

        private void SendRequestToServer(Socket client)
        {
            byte[] dataToSend = Encoding.UTF8.GetBytes("Hello server!");
            client.Send(dataToSend);
        }
    }
}