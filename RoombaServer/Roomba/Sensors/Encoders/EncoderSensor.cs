using System;
using Microsoft.SPOT;
using RoombaServer;
namespace RoombaServer.Roomba.Sensors.Encoders

{
    public class EncoderSensor
    {
        bool firstMeasurement = true;
        int overrunCorrector;
        int prevoiusValue = 0;
      public  int Value { get; set; } = 0;

        public void UpdateData(int newValue)
        {
            if (firstMeasurement)
            {
                prevoiusValue = newValue;
                firstMeasurement = false;
            }

            Value += (newValue - prevoiusValue);  //papildina vçrtîbu par deltaVal
            
            if ((prevoiusValue - newValue) > 10000)//korekcija pie poz overrun
                Value += 65535;

            if ((newValue - prevoiusValue) > 10000)//neg overrun
                Value -= 65535;

            prevoiusValue = newValue;
        }
    }
}
