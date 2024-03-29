﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webserver
{
    class HTTPResponse
    {
        // Коды состояния с соответствующими поясняющими сообщениями
        readonly static Dictionary<string, string> StatusCodes = new Dictionary<string, string>()
        {
            { "200", "OK"},
            { "302", "Redirect"},
            { "400", "Bad Request" },
            { "404",  "Not found"},
            { "405",  "Method Not Allowed"},
            { "500",  "Server error"},
            { "505",  "HTTP Version Not Supported"}
        };
        public HTTPResponse() {}
        string _statusCode;
        // При записи кода состояния поясняющее сообщение находится автоматически
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
        public byte[] Body { get; set; }
        // Поле для управления выводом бинарных данных в сокращенном виде
        public bool isBodyBinary=false;


        public override string ToString()
        {
            string result = Version  + " " + StatusCode + " " + Status + "\r\n";
            foreach (var item in Headers)
            {
                result += item.Key + ": " + item.Value + "\r\n";
            }
            result += "\r\n";
            if (Body!=null)
            {
                if (isBodyBinary)
                {
                    result += "[Binary data]";
                }
                else
                {
                    result += Encoding.ASCII.GetString(Body);
                }
                
            }

            return result;
        }

        // Перевод ответа в форму для передачи по сети
        public byte[] ToBinary()
        {
            string textResult = Version + " " + StatusCode + " " + Status + "\r\n";
            foreach (var item in Headers)
            {
                textResult += item.Key + ": " + item.Value + "\r\n";
            }
            textResult += "\r\n";
            byte[] byteTextResult = Encoding.ASCII.GetBytes(textResult);

            byte[] result;
            if (Body!=null)
            {
                result = new byte[byteTextResult.Length + Body.Length];
                byteTextResult.CopyTo(result, 0);
                Body.CopyTo(result, byteTextResult.Length);
            } else
            {
                result = new byte[byteTextResult.Length];
                byteTextResult.CopyTo(result, 0);
            }
            return result;
        }
    }
}
