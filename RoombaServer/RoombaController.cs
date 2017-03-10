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
            comandExecutor.ExecComand(RoombaComand.Control);
            comandExecutor.ExecComand(RoombaComand.FullControl);

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
            wakeupSignalPort.Write(true);        }
    }
}
