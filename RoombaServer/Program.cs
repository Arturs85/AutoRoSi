using System;
using Microsoft.SPOT;
using System.Threading;
using RoombaServer.Roomba;
using RoombaServer.Networking;
using RoombaServer.Tasks;
using RoombaServer.Roomba;
using RoombaServer.Networking.StatusSending;
using RoombaServer.Tasks;
using RoombaServer.Networking.RemoteCommands;
namespace RoombaServer
{
    public class Program
    {
        public static void Main()
        {
            Program p = new Program();
            p.Start();
            Thread.Sleep(-1);
        }
        private RoombaController roombaController;
        private RoombaStatusSender roombaStatusSender;
        private NetworkManager networkManager;

        private Task currentTask;
        private RemoteCommandReciever remoteCommandReciever;

        private Program()
        {
            roombaController = new RoombaController();
            roombaStatusSender = new RoombaStatusSender(roombaController.Sensors);
            networkManager = new NetworkManager();

            remoteCommandReciever = new RemoteCommandReciever();
            remoteCommandReciever.RemoteCommandRecieved +=
                new RemoteCommandReciever.RemoteCommandRecievedDelegate(remoteCommandReciever_RemoteCommandRecieved);

        }

        public void Start()
        {
            networkManager.Start();
            roombaController.Start();
            roombaStatusSender.Start();
            remoteCommandReciever.Start();
        }
        private void remoteCommandReciever_RemoteCommandRecieved(RemoteCommand remoteCommand)
        {
            StopCurrentTask();
            if (remoteCommand.CommandType == RemoteCommandType.Drive)
            {
                roombaController.CommandExecutor.DriveWheels((short)remoteCommand.FirstParam, (short)remoteCommand.SecondParam);
            }
            else if (remoteCommand.CommandType == RemoteCommandType.ResetLocation)
            {
                //
            }
            else if (remoteCommand.CommandType == RemoteCommandType.Wander)
            {
                currentTask = new TaskWander(roombaController);
                currentTask.Start();
            }
        }
        private void StopCurrentTask()
        {
            if (currentTask != null)
                currentTask.Stop();
        }
    }
}
