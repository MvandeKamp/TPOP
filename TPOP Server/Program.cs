using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace TPOP_Server
{
    class Program
    {
        public static Hashtable clientsList = new Hashtable();
        public static Hashtable accountList = new Hashtable();
        static void Main(string[] args)
        {
            IPAddress localip = NetFuctions.GetLocalIPAddress();
            Console.WriteLine(localip.ToString());
            TcpListener serverSocket = new TcpListener(localip, 7777);
            TcpClient clientSocket = default(TcpClient);
            int counter = 0;

            serverSocket.Start();
            Console.WriteLine("Chat Server Started ....");
            
            counter = 0;
            try
            {
                while (true)
                {
                    counter += 1;
                    clientSocket = serverSocket.AcceptTcpClient();

                    byte[] bytesFrom = new byte[100250];
                    string dataFromClient = null;

                    NetworkStream networkStream = clientSocket.GetStream();
                    networkStream.Read(bytesFrom, 0, (int)clientSocket.ReceiveBufferSize);

                    dataFromClient = System.Text.Encoding.ASCII.GetString(bytesFrom);
                    dataFromClient = dataFromClient.Substring(0, dataFromClient.IndexOf("$"));

                    clientsList.Add(dataFromClient, clientSocket);
                    ClientHandler client = new ClientHandler(clientSocket);
                    client.StartClient();
                }

            } 
            catch(Exception exc)
            {
                LoggingFunctions.WriteToConsole("An Error orcurred. This is the stacktrace: " + exc, ConsoleColor.Red);
            }
            clientSocket.Close();
            serverSocket.Stop();
            Console.WriteLine("exit");
        }

        public class ClientHandler
        {
            TcpClient clientSocket;
            Thread ctThread;
            public ClientHandler(TcpClient client)
            {
                clientSocket = client;
            }
            public void StartClient()
            {
                LoggingFunctions.WriteToConsole("New Clientthread started!", ConsoleColor.Yellow);
                ctThread = new Thread(DoListen);
                ctThread.Start();
            }
            private void DoListen()
            {
                byte[] bytesRecieved = new byte[32];
                string dataFromClient;
                RequestHandler requestHandler;

                while (true)
                {
                    try
                    {
                        NetworkStream networkStream = clientSocket.GetStream();
                        networkStream.Read(bytesRecieved, 0, 32);
                        Console.WriteLine(Encoding.ASCII.GetString(bytesRecieved));
                        Console.WriteLine(Convert.ToInt64(Encoding.ASCII.GetString(bytesRecieved)));
                        bytesRecieved = new byte[Convert.ToInt64(Encoding.ASCII.GetString(bytesRecieved), 2)];
                        networkStream.Read(bytesRecieved, 0, bytesRecieved.Length);
                        Console.WriteLine(Encoding.ASCII.GetString(bytesRecieved));
                        dataFromClient = System.Text.Encoding.ASCII.GetString(bytesRecieved);
                        requestHandler = new RequestHandler(clientSocket, dataFromClient.Substring(0, dataFromClient.IndexOf("$&$REQ$&$") - 9), dataFromClient.Substring(dataFromClient.IndexOf("$&$REQ$&$"), dataFromClient.IndexOf("$&$REQD$&$")));
                        requestHandler.Handle();
                        bytesRecieved = new byte[32];
                    }
                    catch (Exception exc)
                    {
                        LoggingFunctions.WriteToConsole("An error orcurred. This is the stacktrace: " + exc, ConsoleColor.Red);
                    }
                }
            }

            public static void broadcast(string msg, string uName, bool flag)
            {
                foreach (DictionaryEntry Item in clientsList)
                {
                    TcpClient broadcastSocket;
                    broadcastSocket = (TcpClient)Item.Value;

                    NetworkStream broadcastStream = broadcastSocket.GetStream();
                    Byte[] broadcastBytes = null;

                    if (flag == true)
                    {
                        broadcastBytes = Encoding.ASCII.GetBytes(uName + ": " + msg + "$");
                    }
                    else
                    {
                        broadcastBytes = Encoding.ASCII.GetBytes(msg + "$");
                    }
                    Console.WriteLine(broadcastBytes.Length);
                    broadcastStream.Write(broadcastBytes, 0, broadcastBytes.Length);
                    broadcastStream.Flush();
                }

            }
        }
    }
}
