using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using GHIElectronics.NETMF.FEZ;
using System.Threading;
namespace RoombaServer
{
    class RoombaController
    {
        private const String SREIAL_PORT_NAME = "COM2";
        private const int SREIAL_PORT_BAUD_RATE = 115200;
        private SerialPortController serialPortController;
        private RoombaComandExecutor comandExecutor;
        private OutputPort wakeupSignalPort;

        public RoombaComandExecutor CommandExecutor {
            get { return this.comandExecutor; }
        }
        public RoombaController() {
            this.serialPortController = new SerialPortController(SREIAL_PORT_NAME, SREIAL_PORT_BAUD_RATE);
            comandExecutor = new RoombaComandExecutor(serialPortController);
            wakeupSignalPort = new OutputPort((Cpu.Pin)FEZ_Pin.Digital.Di47, true);

        }
        public void Start() {
            SendWakeupSignal();
            comandExecutor.ExecComand(RoombaComand.Start);
            Thread.Sleep(50);
            comandExecutor.ExecComand(RoombaComand.Control);
            Thread.Sleep(50);
            comandExecutor.ExecComand(RoombaComand.FullControl);
            Thread.Sleep(50);

        }
        public void TurnOff()
        {
            comandExecutor.ExecComand(RoombaComand.Safe);
            comandExecutor.ExecComand(RoombaComand.PoverOff);
            serialPortController.Close();
        }
        private void SendWakeupSignal() {
            wakeupSignalPort.Write(false);
            Thread.Sleep(500);
            wakeupSignalPort.Write(true);
        }

        public void SubscribeToSensorPacket(SensorPacket sensorPacket, int sensorPacketsize, int frequency,
            SensorPacketQuerier.SensorDataRecievedDelegate dataRecievedDelegate)
        {
            SensorPacketQuerier querier = new SensorPacketQuerier(comandExecutor, sensorPacket, sensorPacketsize, frequency);
            querier.SensorDataRecieved += dataRecievedDelegate;
            querier.Start();
        }
    }
}
