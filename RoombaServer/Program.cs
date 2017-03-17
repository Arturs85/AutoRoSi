using System;
using Microsoft.SPOT;
using System.Threading;

namespace RoombaServer
{
    public class Program
    {
        public static void Main()
        {
            Debug.Print(Resources.GetString(Resources.StringResources.String1));
            Program p = new Program();
            // p.TestDriving();
           p.TestSensors();
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
            RoombaController controller = new RoombaController();
            controller.Start();
            controller.CommandExecutor.ExecComand(RoombaComand.Safe);
            controller.SubscribeToSensorPacket(SensorPacket.BatteryCharge, 2, 1000, BatteryChargeRecieved);
            controller.SubscribeToSensorPacket(SensorPacket.BumpsWheeldrops, 1, 200, BumpsWheeldropsRecieved);

            Thread.Sleep(-1);

        }
        private void BatteryChargeRecieved(short sensorData) {
            Debug.Print("Battery charge: " + sensorData);

        }
        private void BumpsWheeldropsRecieved(short sensorData) {
            byte bumpRight = 1;
            byte bumpLeft = 1 << 1;
            bool isBumpLeft = (sensorData & bumpLeft) != 0;
            bool isBumpRight = (sensorData & bumpRight) != 0;
            Debug.Print("Bumps Wheeldrops-> bumpLeft: " + isBumpLeft + ", bump right " + isBumpRight);
        }
    }



}
