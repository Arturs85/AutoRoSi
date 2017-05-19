using System;
using Microsoft.SPOT;

namespace RoombaServer.Roomba.Sensors.Encoders
{
     public class EncoderController
    {
        public EncoderController(RoombaController roombaController)
        {
            this.roombaController = roombaController;
            LeftEncoder = new EncoderSensor();
            RightEncoder = new EncoderSensor();
            RobotLocation = new Location();
        }

        double DISTANCE_BETWEEN_WHEELS=300; // ???  
        double ENCODERS_PER_MILLIMETER = 2.526; //???
        bool firstMeasurement = true;
        bool leftEncoderDataRecieved = false;
        bool rightEncoderDataRecieved = false;
        int leftEncoderLastValue =0;
        int rightEncoderLastValue =0;
        RoombaController roombaController;

       public Location RobotLocation { get; set; }

        EncoderSensor LeftEncoder { get; set; }
        int LeftEncoderLastDelta { get; set; }
        double LeftEncoderLastDeltaMillimeters { get; set; }

       EncoderSensor RightEncoder { get; set; }
        int RightEncoderLastDelta { get; set; }
        double RightEncoderLastDeltaMillimeters { get; set; }

       public void Start()
        {
            roombaController.SubscribeToSensorPacket(SensorPacket.RightEncoderCounts, 2, 100, RightEncoderCountsRecieved);
            roombaController.SubscribeToSensorPacket(SensorPacket.LeftEncoderCounts, 2, 100, LeftEncoderCountsRecieved);

        }
       public void ResetToZero()
        {
            RobotLocation.X = 0;
            RobotLocation.Y = 0;
            RobotLocation.HeadingDegrees = 0;
            RobotLocation.HeadingRadians = 0;
            RobotLocation.TotalDistanceFromStartPoint = 0;
        }
        void PerformCalculations()
        {
            LeftEncoderLastDelta = LeftEncoder.Value - leftEncoderLastValue;
            leftEncoderLastValue = LeftEncoder.Value;
            LeftEncoderLastDeltaMillimeters = LeftEncoderLastDelta / ENCODERS_PER_MILLIMETER;

            RightEncoderLastDelta = RightEncoder.Value - leftEncoderLastValue;
            leftEncoderLastValue = LeftEncoder.Value;
            LeftEncoderLastDeltaMillimeters = LeftEncoderLastDelta / ENCODERS_PER_MILLIMETER;

            double deltaNHeading = (RightEncoderLastDeltaMillimeters - LeftEncoderLastDeltaMillimeters) / DISTANCE_BETWEEN_WHEELS;
            RobotLocation.HeadingRadians += deltaNHeading;
            RobotLocation.HeadingDegrees = Common.RadiansToDegrees(RobotLocation.HeadingRadians);

            double deltaNDistance = (RightEncoderLastDeltaMillimeters + LeftEncoderLastDeltaMillimeters) / 2;
            RobotLocation.TotalDistanceFromStartPoint += deltaNDistance;
            //simplified formula
            RobotLocation.X += deltaNDistance * Common.Cos(RobotLocation.HeadingRadians + deltaNHeading / 2); //parametrs radianos?
            RobotLocation.Y += deltaNDistance * Common.Sin(RobotLocation.HeadingRadians + deltaNHeading / 2); //parametrs radianos?


            rightEncoderDataRecieved = false;
            leftEncoderDataRecieved = false;
        }

        void RightEncoderCountsRecieved (short rightEncoderCounts)
        {
            RightEncoder.UpdateData(rightEncoderCounts);
            rightEncoderDataRecieved = true;
            if (leftEncoderDataRecieved)
                PerformCalculations();
        }
  void LeftEncoderCountsRecieved (short leftEncoderCounts)
        {
            LeftEncoder.UpdateData(leftEncoderCounts);
            leftEncoderDataRecieved = true;
            if (rightEncoderDataRecieved)
                PerformCalculations();
            
        }

    }
}
