using System;
using Microsoft.SPOT;

namespace RoombaServer.Roomba.Sensors.Encoders
{
     public class EncoderController
    {
        public EncoderController(RoombaController roombaController)
        {
            this.roombaController = roombaController;
        }

        double DISTANCE_BETWEEN_WHEELS=300; // ???  
        double ENCODERS_PER_MILLIMETER = 10; //???
        bool firstMeasurement = true;
        bool leftEncoderDataRecieved = false;
        bool rightEncoderDataRecieved = false;
        int leftEncoderLastValue =0;
        int rightEncoderLastValue =0;
        RoombaController roombaController;

        Location RobotLocation { get; set; }
        EncoderSensor LeftEncoder { get; set; }
        int LeftEncoderLastDelta { get; }
        double LeftEncoderLastDeltaMillimeters { get; set; }

        EncoderSensor RightEncoder { get; set; }
        int RightEncoderLastDelta { get; }
        double RightEncoderLastDeltaMillimeters { get; set; }

        void Start()
        {
            roombaController.SubscribeToSensorPacket(SensorPacket.RightEncoderCounts, 2, 100, RightEncoderCountsRecieved);
            roombaController.SubscribeToSensorPacket(SensorPacket.LeftEncoderCounts, 2, 100, LeftEncoderCountsRecieved);

        }
        void ResetToZero()
        { }
        void PerformCalculations()
        { }

        void RightEncoderCountsRecieved (short rightEncoderCounts)
        {

        }
  void LeftEncoderCountsRecieved (short leftEncoderCounts)
        {

        }

    }
}
