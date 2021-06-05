using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TPOP_Server
{
    class HelperClasses
    {

    }
    static class NetFuctions
    {
        public static IPAddress GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip;
                }
            }
            throw new Exception("err");
        }
    }

    static class LoggingFunctions
    {
        public static void WriteToConsole(String Message, ConsoleColor consoleColor = ConsoleColor.White)
        {
            Console.ForegroundColor = consoleColor;
            Console.WriteLine("[" + DateTime.Now.TimeOfDay + "]: " + Message);
        }
    }
}
