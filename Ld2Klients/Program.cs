using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static ByteOperations;
namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Debug.Print("klients main");
           
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
                client.Receive(response, response.Length, SocketFlags.None);
                String responseString = "";
                for (int i = 0; i < response.Length; i++)
                {
                    responseString += response[i].ToString() + " ";
                }
                Debug.Print("Atbilde no servera: " + responseString+" Result is: "+bytesToWord(response[0],response[1]));
            }
        }

        private void SendRequestToServer(Socket client)
        {
            Random r = new Random();

            byte[] dataToSend = new byte[3];
            dataToSend[0] = (byte)r.Next(2);
            dataToSend[0]++;
            dataToSend[1] = (byte)r.Next(101);
            dataToSend[2] = (byte)r.Next(101);
            Debug.Print("Sending to Server: " + "darbiba: "+dataToSend[0]+" x= " +dataToSend[1]+" y="+dataToSend[2]);

            client.Send(dataToSend);
        }
    }
}