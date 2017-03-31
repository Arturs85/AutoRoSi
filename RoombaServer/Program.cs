using System;
using Microsoft.SPOT;
using System.Threading;
using RoombaServer.Roomba;
using RoombaServer.Networking;
namespace RoombaServer
{
    public class Program
    {
        RoombaController controller;
        WebServer webServer;
        Thread driveSenseThread;
        byte colisionCount=0;
        short capacity=1;
        short charge = 1;
        Program() {
            //  controller = new RoombaController();
            // controller.Start();
            webServer = new WebServer();
            webServer.SubscribeToButtonInput(HttpButtonClicked);
        }

        public static void Main()
        {
            

            Program p = new Program();

            //  p.TestSensors();
            //p.driveSense();
            // p.TestDriving();
            while (true)
            {
                Thread.Sleep(100);
            }
        }

        void driveSense() {
            // controller = new RoombaController();
         //   controller.Start();
            controller.CommandExecutor.DriveStraight(300);
            while (true) {
                Thread.Sleep(100);
                Debug.Print("colision count : " + colisionCount);

                if (controller.Sensors.IsBump)
                    colisionCount++;
                if (colisionCount == 1) {
                    {

                        controller.CommandExecutor.Stop();
                        Thread.Sleep(100);
                        controller.CommandExecutor.DriveStraight(-100);
                        Thread.Sleep(500);

                        controller.CommandExecutor.TurnRight(200);
                        Thread.Sleep(1800);
                        controller.CommandExecutor.DriveStraight(200);
                        colisionCount++;
                    }
                   
                }
 if (colisionCount > 2) {
                        controller.CommandExecutor.Stop();
                        controller.TurnOff();
                    controller = null;

                }

            }

        }
        private void TestDriving() {
           //controller = new RoombaController();
           // controller.Start();
            controller.CommandExecutor.DriveStraight(300);
            Thread.Sleep(5000);

            controller.CommandExecutor.TurnLeft(200);
            Thread.Sleep(1800);
            controller.CommandExecutor.DriveStraight(300);
            Thread.Sleep(5000);
            controller.CommandExecutor.TurnRight(200);
            Thread.Sleep(1800);
            controller.CommandExecutor.Stop();
            controller.TurnOff();
            controller = null;
        }
        //private void TestSensors() {
        //    //controller.Start();
            
        //    // controller.CommandExecutor.ExecComand(RoombaComand.Safe);
        //    controller.SubscribeToSensorPacket(SensorPacket.BatteryCharge, 2, 2000, BatteryChargeRecieved);
        //    controller.SubscribeToSensorPacket(SensorPacket.BatteryCapacity , 2, 1000, BatteryCapacityRecieved);

        //    controller.SubscribeToSensorPacket(SensorPacket.BumpsWheeldrops, 1, 200, BumpsWheeldropsRecieved);

        //    //Thread.Sleep(-1);

        //}
        private void startController()
        {

            if (controller == null)
            {
                controller = new RoombaController();
            }
                controller.Start();
        
        }

        private void HttpButtonClicked(ButtonNumber buttonNumber) {
            Debug.Print("MainProgram--- User clicked : " + buttonNumber);
            if (buttonNumber == ButtonNumber.StartTask)
            {
                colisionCount = 0;
                startController();
                //  TestSensors();
                driveSenseThread = new Thread(driveSense);
               
                driveSenseThread.Start();
            }
            if(buttonNumber==ButtonNumber.ShutDown && controller!=null)
            {
                controller.CommandExecutor.Stop();
                controller.TurnOff();
                colisionCount = 0;

            }

        }
        private void BatteryChargeRecieved(short sensorData) {
           // Debug.Print("Battery capacity: " + sensorData);
            charge = sensorData;
          //  controller.CommandExecutor.ShowDigitsASCII("123");
           
        }
        private void BatteryCapacityRecieved(short sensorData)
        {
            //Debug.Print("Battery charge: " + sensorData);
            capacity = sensorData;
            //  controller.CommandExecutor.ShowDigitsASCII("123");
            controller.CommandExecutor.ShowDigitsASCII((100*charge/capacity).ToString());
        }

        private void BumpsWheeldropsRecieved(short sensorData) {
            byte bumpRight = 1;
            byte bumpLeft = 1 << 1;
            bool isBumpLeft = (sensorData & bumpLeft) != 0;
            bool isBumpRight = (sensorData & bumpRight) != 0;
           // Debug.Print("Bumps Wheeldrops-> bumpLeft: " + isBumpLeft + ", bump right " + isBumpRight);
          //  Debug.Print("colision count: " + colisionCount);

            if (isBumpLeft || isBumpRight)
            {
                colisionCount++;
                Thread.Sleep(1000);

            }
        }
    }



}
