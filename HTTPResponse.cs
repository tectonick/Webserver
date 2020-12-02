using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webserver
{
    class HTTPResponse:HTTPObject
    {
        static Dictionary<string, string> StatusCodes = new Dictionary<string, string>()
        {
            { "200", "OK"},
            { "404",  "Not found"},
            { "500",  "Server error"}
        };
        public HTTPResponse()
        {

        }
        string _statusCode;
        public string StatusCode { 
            get {
                return _statusCode;
            }
            set {
                string statusMessage;
                if (StatusCodes.TryGetValue(value, out statusMessage))
                {
                    _statusCode = value;
                    _status = statusMessage;
                }
                else
                {
                    throw new Exception("Unknown status code");
                }
            }
        }

        string _status;
        public string Status { get => _status; }
        public string Version { get; set; } = "HTTP/1.1";
        public Dictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();
        public string Body { get; set; }

        public override string ToString()
        {
            string result = StatusCode + " " + Status + " " + Version + "\n";
            foreach (var item in Headers)
            {
                result += item.Key + ": " + item.Value + "\n";
            }
            result += "\n";
            result += Body;
            return result;
        }
    }
}
