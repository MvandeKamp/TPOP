using System;
using System.Collections.Generic;
using System.Text;

namespace TPOP_Server
{
    class RequestHandler
    {
        string RequestType { get; set; }
        string RequestData { get; set; }
        public RequestHandler(string requestType, string requestData)
        {
            RequestType = requestType;
            RequestData = requestData;
        }
        public string Handle()
        {
            switch (RequestType)
            {
                case "createAccount":
                    return "1";
                default:
                    return "0";
            }
        }
    }
}
