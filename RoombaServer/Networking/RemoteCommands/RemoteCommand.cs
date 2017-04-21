using System;
using Microsoft.SPOT;

namespace RoombaServer.Networking.RemoteCommands
{
   public class RemoteCommand
    {
        public RemoteCommandType CommandType
        {
            get;
            set;
         }

        public int FirstParam
        {
            get;
            set;
        }
public int SecondParam
        {
            get;
            set;
        }
        public RemoteCommand()
        {
        }
        public RemoteCommand(RemoteCommandType commandType, int firstParam, int secondParam)
        {
            CommandType = commandType;
            FirstParam = firstParam;
            SecondParam = secondParam;

        }

    }
}
