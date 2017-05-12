using System;
using Microsoft.SPOT;
using System.IO.Ports;
using System.Threading;

namespace RoombaServer.Roomba
{
   public class SerialPortController
    {
        private SerialPort serialPort;
        private static Object readWriteLock = new object();
        public SerialPortController(String portName, int baudRate)
        {
            serialPort = new SerialPort(portName, baudRate);
            Open();
            this.serialPort.DiscardInBuffer();
            this.serialPort.DiscardOutBuffer();
            this.serialPort.Flush();
        }
        private void Open()
        {
            if (!serialPort.IsOpen)
                serialPort.Open();
        }
        public void Write(byte[] dataToWrite)
        {
            lock (readWriteLock)
            {
                Open();
                serialPort.Write(dataToWrite, 0, dataToWrite.Length);
                //Thread.Sleep(50);
            }
        }
        public byte[] Read(int bytesToRead)
        {
            lock (readWriteLock)
            {
                Open();
                int offset = 0;
                int bytesRead = 0;
                    byte[] data = new byte[bytesToRead];

                while (offset < bytesToRead)
                {
                   bytesRead =  serialPort.Read(data, offset, data.Length-offset);
                    offset += bytesRead;
                }
                    return data;
            }
        }
        public void Close()
        {

            if (serialPort.IsOpen)
                serialPort.Close();
        }


    }
}
