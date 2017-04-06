using System;
using Microsoft.SPOT;
using RoombaServer.Roomba;
using System.Threading;

namespace RoombaServer.Tasks
{
   public class TaskWander:Task
    {

        Random rnd;
        public TaskWander(RoombaController roombaController):base(roombaController) {
            rnd = new Random();
        }

        protected override void DoWork()
        {
            roombaController.CommandExecutor.DriveStraight(300);
            while (!stop)
            {
                Thread.Sleep(100);
              //  Debug.Print("colision count : " + colisionCount);

                if (roombaController.Sensors.IsBump)

                {
                    roombaController.CommandExecutor.Stop();
                    Thread.Sleep(100);
                    roombaController.CommandExecutor.DriveStraight(-100);
                    Thread.Sleep(500);
                    int turningTime = rnd.Next(1500) + 300;

                    roombaController.CommandExecutor.TurnRight(200);
                    Thread.Sleep(turningTime);
                    roombaController.CommandExecutor.DriveStraight(200);
                  
                }
               
            }
            roombaController.CommandExecutor.Stop();
        }
    }
}
