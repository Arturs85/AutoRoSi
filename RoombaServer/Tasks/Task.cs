using System;
using Microsoft.SPOT;
using RoombaServer.Roomba;
using System.Threading;
namespace RoombaServer.Tasks
{
   public abstract class Task
    {
        protected RoombaController roombaController;
        protected bool stop;
        private Thread workingThread;
        public Task(RoombaController roombaController)
        {
            this.roombaController = roombaController;
            stop = true;

        }
        public void Start()
        {
            if (stop)
            {
                stop = false;
                workingThread = new Thread(DoWork);
                workingThread.Start();
            }
        }
        public void Stop()
        {
            stop = true;
        }
        protected abstract void DoWork();
    }
}
