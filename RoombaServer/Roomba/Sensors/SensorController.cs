using System;
using Microsoft.SPOT;

namespace RoombaServer.Roomba.Sensors
{
  public  class SensorController
    {
        private short batteryCharge;
        private RoombaController roombaController;
        public int BatteryPercentage {
            get;
            private set;
        }
        public bool IsBump {
            get;
            private set;

        }
        public byte BumpsWheeldropsData {
            get;
            private set;
        }
        public SensorController(RoombaController roombaController)
        {
            this.roombaController = roombaController;

        }
        public void StartSensors()
        {
            roombaController.SubscribeToSensorPacket(SensorPacket.BatteryCharge, 2, 1000, BatteryChargeRecieved);
            roombaController.SubscribeToSensorPacket(SensorPacket.BatteryCapacity, 2, 1000, BatteryCapacityRecieved);

            roombaController.SubscribeToSensorPacket(SensorPacket.BumpsWheeldrops, 1, 20, BumpsWheeldropsRecieved);
            
        }

        private void BumpsWheeldropsRecieved(short sensorData)
        {
            byte bumpRight = 1;
            byte bumpLeft = 1 << 1;
            bool isBumpLeft = (sensorData & bumpLeft) != 0;
            bool isBumpRight = (sensorData & bumpRight) != 0;

            IsBump = isBumpLeft || isBumpRight;
            BumpsWheeldropsData = (byte)sensorData;
        }

        private void BatteryChargeRecieved(short sensorData) {
            batteryCharge = (short)sensorData;
        }
        private void BatteryCapacityRecieved(short sensorData)
        {
            if (sensorData != 0)
                BatteryPercentage = 100 * batteryCharge / sensorData;
            else
                BatteryPercentage = 0;
            Debug.Print("Battery charge: " + BatteryPercentage.ToString());

            roombaController.CommandExecutor.ShowDigitsASCII(BatteryPercentage.ToString());
        }
    }
}
