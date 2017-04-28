using System;
using Microsoft.SPOT;

namespace RoombaServer.Roomba.Sensors.Encoders
{
   public class EncoderSensor
    {
        bool firstMeasurement;
        int overrunCorrector;
        int Value { get; set; }

        void UpdateData(int newValue)
        {

        }
    }
}
