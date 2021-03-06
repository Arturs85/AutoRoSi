﻿using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using System.Threading;
using GHIElectronics.NETMF.Net;
using GHIElectronics.NETMF.Net.NetworkInformation;
using GHIElectronics.NETMF.Net.Sockets;
using GHIElectronics.NETMF.FEZ;
using System.Text;

namespace Server
{
    public class Program
    {


        Thread pingThread;
        Thread serverThread;
        private bool isLedLit = false;
        private static OutputPort led;


        public  void Setup()
        {
            // Izveido ieejas portu uz norādītās CPU kājas 
           

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

        private byte[] PrepareResponse(HttpListenerContext context)
        {
            byte[] response = Encoding.UTF8.GetBytes(
            @"<html>
            <title>Embedded Web Server</title>
                <body>
                    <hl>This comes from FEZ Panda II</hl>
                   <div>This is some text</div>
                </body>
            </html>");
            return response;
        }

        private byte[] PrepareResponseV2(HttpListenerContext context)
        {
            bool buttonOnePressed = false;
            bool buttonTwoPressed = false;

            if (context.Request.HttpMethod == "POST")
            {
                // from submitted form we get "buttonTwo=Button+Two+%3A%28" if second button is pressed
                String contentstring = this.GetContentString(context.Request);

                buttonOnePressed = contentstring.IndexOf("buttonOne") != -1;

                buttonTwoPressed = contentstring.IndexOf("buttonTwo") != -1;
            }

            // TODO extract to resources
            String responseString =
                  @"<html>
                    <title>Embedded Web Server</title>
                    <body>
                        <form action= """" method=""post"">
                            <hl>This comes from FEZ Panda II</hl>
                            <div>This is some text</div>";

            if (buttonOnePressed)
            {
                responseString += @"<div style=""color:red"">You pressed button one!</div>";
            }
            else if (buttonTwoPressed)
            {
                responseString += @"<div style=""color:red"">You pressed button two!</div>";
            }

            responseString += @"
                            <div><input type=""submit"" name=""buttonOne"" value=""Button One :)""/></div>
                            <div><input type=""submit"" name=""buttonTwo"" value=""Button Two :(""/></div>
                        </form>
                    </body>
                </html>";

            return Encoding.UTF8.GetBytes(responseString);
        }

        /**  Control a LED */
        private byte[] PrepareResponseV3LED(HttpListenerContext context)
        {
            bool buttonPressed = false;

            if (context.Request.HttpMethod == "POST")
            {
                // from submitted form we get "buttonTwo=Button+Two+%3A%28" if second button is pressed
                String contentstring = this.GetContentString(context.Request);

                buttonPressed = contentstring.IndexOf("ledBtn") != -1;
            }

            
            String responseString =
                  @"<html>
                    <title>-= LED  =-</title>
                    <body>
                        <form action= """" method=""post"">
                            <hl>This comes from FEZ Panda II</hl>
                            <div>This is some text</div>";

            if (buttonPressed)
            {
                // toggle led
                led.Write(!led.Read());
                Debug.Print("Button pressed");
                string ledState;
                if (led.Read())
                    ledState = "On";
                else
                    ledState = "Off";
                responseString += @"<div style=""color:red"">Led is "+ledState +"</div>";
            }

            responseString += @"
                            <div><input type=""submit"" name=""ledBtn"" value=""Button One :)""/></div>
                           
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
                new IPAddress(new byte[] { 192, 168, 17, 1 }), // ИП роутера
                80); // http порт

            while (true)
            {
                // открываем сокет для передачи информации
                Socket socket = new Socket(AddressFamily.InterNetwork,
                    SocketType.Dgram, ProtocolType.Udp);    // UDP

                for (int i = 0; i < 30; i++)
                {
                    // шлём роутеру (reouterEndPoint) данные через наш сокет
                    socket.SendTo(new byte[] { 1 }, routerEndPoint);
                    socket.SendTo(new byte[] { 1, 2 }, routerEndPoint);
                    socket.SendTo(new byte[] { 1, 2, 3 }, routerEndPoint);
                    socket.SendTo(new byte[] { 1, 2, 3, 4 }, routerEndPoint);
                    socket.SendTo(new byte[] { 1, 2, 3, 4, 5 }, routerEndPoint);
                    Thread.Sleep(100);
                }

                // закрываем соединение
                socket.Close();
                Thread.Sleep(5000);
            }
        }

        private void startPing()
        {
            // работаем с сетью в отдельном потоке
            pingThread = new Thread(this.Ping);
            pingThread.Start();
        }

        public static void Main()
        {
            Program p = new Program();
            p.Setup();
            p.EnableNetworking();
            p.startPing();
            p.StartWebServer();
        }

    }
}