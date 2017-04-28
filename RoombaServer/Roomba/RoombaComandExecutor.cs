using System;
using Microsoft.SPOT;

namespace RoombaServer.Roomba
{
   public class RoombaComandExecutor
    {
        SerialPortController serialPortController;
        private static Object querrySensorLock = new object();
        public RoombaComandExecutor(SerialPortController serialPort)
        {
            serialPortController = serialPort;
        }
        public void ExecGeneralCommand(byte[] command)
        {
            serialPortController.Write(command);

        }

        public void ExecComand(RoombaComand comand, byte[] parameters)
        {
            byte[] commandBytes = new byte[parameters.Length + 1];
            commandBytes[0] = (byte)comand;
            parameters.CopyTo(commandBytes, 1);
            ExecGeneralCommand(commandBytes);
        }
        public void ExecComand(RoombaComand comand)
        {
            byte[] commandBytes = new byte[] { (byte)comand };
            ExecGeneralCommand(commandBytes);
        }
        public void Drive(short velocity, short radius)
        {
            byte[] parameters = {
                Common.highByteFromWord(velocity),
            Common.lowByteFromWord(velocity),
            Common.highByteFromWord(radius),
            Common.lowByteFromWord(radius)
            };
            ExecComand(RoombaComand.Drive, parameters);
        }
        public void DriveStraight(short velocity)
        {
            Drive(velocity, 32767);
        }
        public void TurnRight(short velocity)
        {
            Drive(velocity, -1);
        }
        public void TurnLeft(short velocity)
        {
            Drive(velocity, 1);
        }
        public void Stop()
        {
            Drive(0, 0);
        }

        public byte[] QuerrySensorPacket(SensorPacket sensorPacket, int packetSize) {
            byte[] parameters = new byte[] { (byte)sensorPacket };
            lock (querrySensorLock) {
                ExecComand(RoombaComand.QuerrySensorPacket, parameters);
                return serialPortController.Read(packetSize);

            } 

        }
        public void ShowDigitsASCII(String value) {
            if (value == null) return;
            byte[] parameters = new byte[4];
            for (int i = 0; i < parameters.Length; i++)
            {
                if (i <= value.Length - 1)
                {
                    parameters[i] = (byte)value[i];
                }
                else
                    parameters[i] = 32;
            }
            ExecComand(RoombaComand.DigitLedsASCII, parameters);
        }

        public void DriveWheels(short leftWheelSpeed, short rightWheelSpeed ) //tested
        {
            byte[] parameters = {
                Common.highByteFromWord(rightWheelSpeed),
            Common.lowByteFromWord(rightWheelSpeed),
            Common.highByteFromWord(leftWheelSpeed),
            Common.lowByteFromWord(leftWheelSpeed)
            };
            ExecComand(RoombaComand.DriveWheels, parameters);
        }


    }
}
