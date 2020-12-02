using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webserver
{
    class HTTPRequest:HTTPObject
    {
        public HTTPRequest(string data)
        {

        }
        public string Method { get; set; }
        public string Path { get; set; }
        public string Version { get; set; }
        public Dictionary<string, string> Headers { get; set; } =  new Dictionary<string, string>();
        public string Body { get; set; }

        public override string ToString()
        {
            string result = Method + " " + Path + " " + Version + "\n";
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
