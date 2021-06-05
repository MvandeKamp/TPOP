using GameStateLib;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace TPOP_Server
{
    class RequestHandler
    {
        TcpClient Sender { get; set; }
        string RequestType { get; set; }
        string RequestData { get; set; }
        public RequestHandler(TcpClient sender, string requestType, string requestData)
        {
            Sender = sender;
            RequestType = requestType;
            RequestData = requestData;
        }
        public void Handle()
        {
            switch (RequestType)
            {
                case "createAccount":
                    CreateAccount();
                    break;
                case "loginAccount":
                    LoginAccount();
                    break;
                default:
                    break;
            }
        }
        private void CreateAccount()
        {
            JMessage message = JMessage.Deserialize(RequestData);
            Player player = message.Value.ToObject<Player>();
            if (Program.accountList.ContainsKey(player.Username))
            {
                ReturnToRequester("notification", "Your account already exists");
            } else
            {
                ReturnToRequester("account", JMessage.Serialize(JMessage.FromValue(player)));
            }
        }

        private void LoginAccount()
        {
            JMessage message = JMessage.Deserialize(RequestData);
            Player player = message.Value.ToObject<Player>();
            if (!Program.accountList.ContainsKey(player.Username))
            {
                ReturnToRequester("notification", "Your account does not exist");
            } else
            {
                ReturnToRequester("account", JMessage.Serialize(JMessage.FromValue(player)));
            }
        }

        public async void ReturnToRequester(string command, string data)
        {
            string dataString = command + "$&$REQ$&$" + data + "$&$REQD$&$";
            byte[] outStream = new byte[dataString.Length + 64];
            outStream = Encoding.ASCII.GetBytes(outStream.Length + dataString);
            NetworkStream returnStream = Sender.GetStream();
            await returnStream.WriteAsync(outStream, 0, outStream.Length);
        }
    }
}
