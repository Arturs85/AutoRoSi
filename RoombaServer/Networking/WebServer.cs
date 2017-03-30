using System;
using Microsoft.SPOT;

using Microsoft.SPOT.Hardware;
using System.Threading;
using GHIElectronics.NETMF.Net;
using GHIElectronics.NETMF.Net.NetworkInformation;
using GHIElectronics.NETMF.Net.Sockets;
using GHIElectronics.NETMF.FEZ;
using System.Text;

namespace RoombaServer.Networking
{
    class WebServer
    {

        Thread pingThread;
        Thread serverThread;
        private bool isLedLit = false;
        private static OutputPort led;
        public delegate void ClientInputRecievedDelegate(ButtonNumber buttonNumber);
        public event ClientInputRecievedDelegate ClientInputRecieved;

        private void Setup()
        {
            // Izveido ieejas portu uz norâdîtâs CPU kâjas 


            led = new OutputPort(
                (Cpu.Pin)FEZ_Pin.Digital.LED,
                false);

        }

        private void StartWebServer()
        {
            // izvietot HttpListener objektu uz 80. porta
            HttpListener httpListener = new HttpListener("http", 80);
            // sakt klausities
            httpListener.Start();

            HttpListenerContext httpContext = null;

            while (true)
            {
                try
                {
                    // wait for incoming request
                    // iegut context'u kas satur pieprasijumu
                    httpContext = httpListener.GetContext();
                    if (httpContext == null) continue;

                    // sagatabot atbildi
                    byte[] response = this.PrepareResponseV3LED(httpContext);
                    // nosutit atbildi
                    this.SendResponse(httpContext, response);
                }
                catch (Exception e)
                {
                    Debug.Print("Error processing listener context: " + e.Message);
                }
                finally
                {
                    if (httpContext != null)
                    {
                        httpContext.Close();
                    }
                }
            }
        }

       

        /**  Control a LED */
        private byte[] PrepareResponseV3LED(HttpListenerContext context)
        {
            bool buttonStartTaskPressed = false;
            bool buttonStopPresed = false;
            if (context.Request.HttpMethod == "POST")
            {
                // from submitted form we get "buttonTwo=Button+Two+%3A%28" if second button is pressed
                String contentstring = this.GetContentString(context.Request);

                buttonStartTaskPressed = contentstring.IndexOf("StartTask") != -1;
                buttonStopPresed = contentstring.IndexOf("ShutDown") != -1;
                if ( buttonStartTaskPressed)
                    ClientInputRecieved(ButtonNumber.StartTask);
                if (buttonStopPresed)
                    ClientInputRecieved(ButtonNumber.ShutDown);
            }


            String responseString =
                  @"<html>
                    <title>-= Roomba Control Panel  =-</title>
                    <body>
                        <form action= """" method=""post"">
                            <hl>This comes from FEZ Panda II</hl>
                            <div>Controller </div>";

            if (buttonStartTaskPressed)
            {
                // toggle led
                led.Write(!led.Read());
                Debug.Print("WebServer - Button pressed");
                string ledState;
                if (led.Read())
                    ledState = "On";
                else
                    ledState = "Off";
                responseString += @"<div style=""color:red"">Roomba is " + ledState + "</div>";
            }

            responseString += @"
                            <div><input type=""submit"" name=""StartTask"" value=""Start Task""/></div>
                           <div><input type=""submit"" name=""ShutDown"" value=""Shut Down""/></div>
                        </form>
                    </body>
                </html>";

            return Encoding.UTF8.GetBytes(responseString);
        }

        private String GetContentString(HttpListenerRequest request)
        {
            byte[] requestData = new byte[(int)request.InputStream.Length];
            request.InputStream.Read(requestData, 0, requestData.Length);
            return new String(Encoding.UTF8.GetChars(requestData));
        }

        private void SendResponse(HttpListenerContext context, byte[] response)
        {
            // html status code
            context.Response.StatusCode = 200;
            // informet klientu ka atbilde ir html kods
            context.Response.ContentType = "text/html";
            try
            {
                // ierakstit sagatavotus datus atbildes plusmaa
                context.Response.OutputStream.Write(response, 0, response.Length);
            }
            finally
            {
                // send and close
                context.Response.Close();
            }
        }

        /** Configure Wi-Fi shield and setup internet settings */
        private void EnableNetworking()
        {
            WIZnet_W5100.Enable(SPI.SPI_module.SPI1,
                (Cpu.Pin)FEZ_Pin.Digital.Di10,
                (Cpu.Pin)FEZ_Pin.Digital.Di7,
                false);


            byte[] ipAddress = new byte[] { 192, 168, 17, 41 };
            byte[] netmask = new byte[] { 255, 255, 255, 0 };
            byte[] gateway = new byte[] { 192, 168, 17, 1 };
            // set our own mac address for this device
            byte[] macAddress = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x41 };

            NetworkInterface.EnableStaticIP(ipAddress, netmask, gateway, macAddress);
        }
        private void Ping()
        {
            // target endpoint
            IPEndPoint routerEndPoint = new IPEndPoint(
                new IPAddress(new byte[] { 192, 168, 17, 1 }), // ?? ???????
                80); // http ????

            while (true)
            {
                // ????????? ????? ??? ???????? ??????????
                Socket socket = new Socket(AddressFamily.InterNetwork,
                    SocketType.Dgram, ProtocolType.Udp);    // UDP

                for (int i = 0; i < 30; i++)
                {
                    // ???? ??????? (reouterEndPoint) ?????? ????? ??? ?????
                    socket.SendTo(new byte[] { 1 }, routerEndPoint);
                    socket.SendTo(new byte[] { 1, 2 }, routerEndPoint);
                    socket.SendTo(new byte[] { 1, 2, 3 }, routerEndPoint);
                    socket.SendTo(new byte[] { 1, 2, 3, 4 }, routerEndPoint);
                    socket.SendTo(new byte[] { 1, 2, 3, 4, 5 }, routerEndPoint);
                    Thread.Sleep(100);
                }

                // ????????? ??????????
                socket.Close();
                Thread.Sleep(5000);
            }
        }

        private void startPing()
        {
            // ???????? ? ????? ? ????????? ??????
            pingThread = new Thread(this.Ping);
            pingThread.Start();
        }

        public  void Start()
        {
           
            Setup();
            EnableNetworking();
            startPing();
            StartWebServer();
        }
        public void SubscribeToButtonInput(ClientInputRecievedDelegate inputRecievedDelegate)
        {
           
            ClientInputRecieved += inputRecievedDelegate;
            Start();
        }


    }
}

