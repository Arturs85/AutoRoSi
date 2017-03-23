using System;
using Microsoft.SPOT;
using System.Threading;

namespace RoombaServer
{
    public class Program
    {
        RoombaController controller;
        byte colisionCount=0;
        Program() {
            controller = new RoombaController();
            controller.Start();
        }

        public static void Main()
        {
            

            Debug.Print(Resources.GetString(Resources.StringResources.String1));
            Program p = new Program();
                       p.TestSensors();
            p.driveSense();
            // p.TestDriving();
        }

        void driveSense() {
            while (true) {

                if (colisionCount == 1) {
                    {
                        controller.CommandExecutor.Stop();
                        Thread.Sleep(100);
                        controller.CommandExecutor.DriveStraight(-100);
                        Thread.Sleep(500);

                        controller.CommandExecutor.TurnRight(200);
                        Thread.Sleep(1800);
                        controller.CommandExecutor.DriveStraight(200);
                        colisionCount = 2;
                    }
                    if (colisionCount > 2) {
                        controller.CommandExecutor.Stop();
                        controller.TurnOff();

                    }
                }


            }

        }
        private void TestDriving() {
            RoombaController controller = new RoombaController();
            controller.Start();
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

        }
        private void TestSensors() {
           
            controller.CommandExecutor.ExecComand(RoombaComand.Safe);
            controller.SubscribeToSensorPacket(SensorPacket.BatteryCharge, 2, 1000, BatteryChargeRecieved);
            controller.SubscribeToSensorPacket(SensorPacket.BumpsWheeldrops, 1, 200, BumpsWheeldropsRecieved);

            Thread.Sleep(-1);

        }
        private void BatteryChargeRecieved(short sensorData) {
            Debug.Print("Battery charge: " + sensorData);
            if (sensorData < 1000)
                sensorData = 1000;
            if (sensorData > 2700)
                sensorData = 2700;
            int sensorDataint = sensorData - 1000;
            sensorData = (short)(sensorDataint / 17);

            controller.CommandExecutor.ShowDigitsASCII(sensorData.ToString());
        }
        private void BumpsWheeldropsRecieved(short sensorData) {
            byte bumpRight = 1;
            byte bumpLeft = 1 << 1;
            bool isBumpLeft = (sensorData & bumpLeft) != 0;
            bool isBumpRight = (sensorData & bumpRight) != 0;
            Debug.Print("Bumps Wheeldrops-> bumpLeft: " + isBumpLeft + ", bump right " + isBumpRight);
            if (isBumpLeft || isBumpRight)
            {
                colisionCount++;
                Thread.Sleep(1000);
            }
        }
    }



}
