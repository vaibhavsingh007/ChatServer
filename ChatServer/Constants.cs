using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer
{
    internal static class Constants
    {
        public static class ConnectionSymbols
        {
            public const string EndConnection = "|FIN|";
            public const string SuccessfulConnection = "|ACK|";
            public const string UnsuccessfulConnection = "|NACK|";
            public const string ServerId = "|SRVR|";
        }

        public static class ChatSymbols
        {
            public const string ForClientIdDelimiter = "|~~|";
        }
    }
}
