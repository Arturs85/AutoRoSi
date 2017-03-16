using System;
using Microsoft.SPOT;
using System.Threading;

namespace RoombaServer
{
    class SensorPacketQuerier
    {
        private RoombaComandExecutor comandExecutor;
        private SensorPacket sensorPacket;
        private int sensorPacketSize;
        private int frequency;
        private bool stop;

        public delegate void SensorDataRecievedDelegate(short sensorData);
        public event SensorDataRecievedDelegate SensorDataRecieved;

        public SensorPacketQuerier(RoombaComandExecutor comandExecutor, SensorPacket sensorPacket, int sensorPacketSize, int frequency) {
            this.comandExecutor = comandExecutor;
            this.sensorPacket = sensorPacket;
            this.sensorPacketSize = sensorPacketSize;
            this.frequency = frequency;
        }
        public void Start() {
            stop = false;
            new Thread(DoWork).Start();
            
        }
        public void Stop() {
            stop = true;
        }

        private void DoWork() {
            while (!stop) {
                byte[] sensorData = comandExecutor.QuerrySensorPacket(sensorPacket, sensorPacketSize);
                short sensorDataShort = 0;
                if (sensorPacketSize == 1)
                {
                    sensorDataShort = sensorData[0];
                }
                else if (sensorPacketSize == 2) {
                    sensorDataShort = Common.bytesToWord(sensorData[0], sensorData[1]);
                }
                if (SensorDataRecieved != null) {
                    SensorDataRecieved(sensorDataShort);
                }
                Thread.Sleep(frequency);
            }

        }
    }


}
