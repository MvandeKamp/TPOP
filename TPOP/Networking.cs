using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GameStateLib;
namespace TPOP
{
    public static class Networking
    {
        public static NetworkStream serverStream;
        public static TcpClient clientSocket = new TcpClient();
        public static void Connect(string serverIP, string serverPort)
        {
            clientSocket.Connect(serverIP, Int32.Parse(serverPort));
            serverStream = clientSocket.GetStream();
        }
        public static void StartReading()
        {
            Thread ctThread = new Thread(GetMessage);
            ctThread.Start();
        }
        private static void GetMessage()
        {
            while (true)
            {
                serverStream = clientSocket.GetStream();
                int buffSize = 0;
                byte[] inStream = new byte[65536];
                buffSize = clientSocket.ReceiveBufferSize;
                serverStream.Read(inStream, 0, buffSize);
                string returndata = System.Text.Encoding.ASCII.GetString(inStream);
                //readData = returndata.Substring(0, returndata.IndexOf("$"));
                //Msg(readData);
                Console.WriteLine(buffSize);
            }
        }
        public static void CreateAccount(Player player)
        {
            SendData("createAccount", JMessage.Serialize(JMessage.FromValue(player)));
        }
        public static void LoginAccount(Player player)
        {
            SendData("loginAccount", JMessage.Serialize(JMessage.FromValue(player)));
        }
        private static async void SendData(string command, string data)
        {
            string dataString = command + "$&$REQ$&$" + data + "$&$REQD$&$";
            byte[] outStream = new byte[dataString.Length + 32];
            Console.WriteLine(Convert.ToString(outStream.Length, 2).Length);
            Console.WriteLine(Convert.ToString(outStream.Length, 2).PadLeft(32, '0'));
            outStream = Encoding.ASCII.GetBytes(Convert.ToString(outStream.Length, 2).PadLeft(32, '0') + dataString);
            await serverStream.WriteAsync(outStream, 0, outStream.Length);
        }
    }
}
