using System;
using Microsoft.SPOT;
using System.IO.Ports;
using System.Threading;

namespace RoombaServer
{
    class SerialPortController
    {
        private SerialPort serialPort;
            private static Object readWriteLock = new object();
        public SerialPortController(String portName, int baudRate) {
            serialPort = new SerialPort(portName, baudRate);
            Open();
        }
        private void Open() {
            if (!serialPort.IsOpen)
                serialPort.Open();
        }
        public void Write(byte[] dataToWrite) {
            lock (readWriteLock) {
                Open();
                serialPort.Write(dataToWrite, 0, dataToWrite.Length);
                Thread.Sleep(50);
            }
        }
  public byte[] Read(int bytesToRead) {
            lock (readWriteLock) {
                Open();
                byte[] data = new byte[bytesToRead];
                serialPort.Read(data, 0, data.Length);
                return data;
            }
        }
        public void Close() {

            if (serialPort.IsOpen)
                serialPort.Close();
        }


    }
}
