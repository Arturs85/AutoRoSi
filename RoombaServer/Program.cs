using System;
using Microsoft.SPOT;
using System.Threading;
using RoombaServer.Roomba;
using RoombaServer.Networking;
using RoombaServer.Tasks;
using RoombaServer.Roomba;
using RoombaServer.Networking.StatusSending;
using RoombaServer.Tasks;
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

        private Program()
        {
            roombaController = new RoombaController();
            roombaStatusSender = new RoombaStatusSender(roombaController.Sensors);
            networkManager = new NetworkManager();

        }

        public void Start()
        {
            networkManager.Start();
            roombaController.Start();
            roombaStatusSender.Start();

        }
    }
}
