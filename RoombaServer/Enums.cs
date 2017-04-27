using System;
using Microsoft.SPOT;

namespace RoombaServer
{
   public enum RoombaComand// class Enums
    {
        Start = 128,
        Control = 130,
        Safe = 131,
        FullControl = 132,
        PoverOff = 133,
        Drive = 137,
        DigitLedsASCII = 164,
QuerrySensorPacket = 142,
DriveWheels = 145
    };
    public enum SensorPacket {
        BumpsWheeldrops = 7,
        BatteryCharge = 25,
BatteryCapacity =26,
RightEncoderCounts = 43,
LeftEncoderCounts = 44
    };
    public enum ButtonNumber
    {
        ShutDown = 1,
        StartTask = 2
       
    };
    public enum RemoteCommandType
    {
        Drive = 1,
        ResetLocation = 2,
        Wander = 3,
        StopCurrentTask = 254,
        SpecialTask = 255

    };


}
