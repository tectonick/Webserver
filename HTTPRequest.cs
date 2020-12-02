using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webserver
{
    class HTTPRequest
    {
        public HTTPRequest(string data)
        {
            try
            {
                var lines = data.Split(new[] { "\r\n" }, StringSplitOptions.None).ToList<string>();
                string[] first = lines[0].Split(new[] { ' ' });
                Method = first[0];
                Path = first[1];
                Version = first[2];
                int endOfHeaders = lines.IndexOf("");
                for (int i = 1; i < endOfHeaders; i++)
                {
                    string[] line = lines[i].Split(new[] { ':' });
                    for (int j = 2; j < line.Length; j++)
                    {
                        line[1] += ":" + line[j];
                    }
                    Headers.Add(line[0], line[1].Trim());
                }
                Body = "";
                for (int i = endOfHeaders + 1; i < lines.Count; i++)
                {
                    Body += lines[i] + "\r\n";
                }
            }
            catch (Exception)
            {
                throw new ArgumentException("Parsing error");
            }
            
        }
        public string Method { get; set; }
        public string Path { get; set; }
        public string Version { get; set; }
        public Dictionary<string, string> Headers { get; set; } =  new Dictionary<string, string>();
        public string Body { get; set; }

        public byte[] BinaryBody { get; set; }

        public override string ToString()
        {
            string result = Method + " " + Path + " " + Version + "\r\n";
            foreach (var item in Headers)
            {
                result += item.Key + ": " + item.Value + "\r\n";
            }
            result += "\r\n";
            result += Body;
            result += "\r\n\r\n";
            return result;
        }

        
    }
}
