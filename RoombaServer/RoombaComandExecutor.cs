using System;
using Microsoft.SPOT;

namespace RoombaServer
{
    class RoombaComandExecutor
    {
        SerialPortController serialPortController;
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
    }
}
