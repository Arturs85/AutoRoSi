using System;
using Microsoft.SPOT;
using System.Threading;

namespace RoombaServer
{
    public class Program
    {
        public static void Main()
        {
            Debug.Print(Resources.GetString(Resources.StringResources.String1));
            Program p = new Program();
            p.TestDriving();
        }
        private void TestDriving() {
            RoombaController controller = new RoombaController();
            controller.Start();
            controller.CommandExecutor.DriveStraight(300);
            Thread.Sleep(5000);

            controller.CommandExecutor.TurnLeft(200);
            Thread.Sleep(1800);
            controller.CommandExecutor.DriveStraight(300);
            Thread.Sleep(5000);
            controller.CommandExecutor.TurnRight(200);
            Thread.Sleep(1800);
            controller.CommandExecutor.Stop();
            controller.TurnOff();

        }
    }

}
