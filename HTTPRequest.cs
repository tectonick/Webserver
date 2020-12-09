using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webserver
{
    class HTTPRequest
    {
        // Конструктор и парсер запроса из текстовой формы
        public HTTPRequest(string data)
        {
            try
            {
                var lines = data.Split(new[] { "\r\n" }, StringSplitOptions.None).ToList<string>();
                // Обработка первой строки
                string[] first = lines[0].Split(new[] { ' ' });
                Method = first[0];
                string pathAndQueryString= first[1];
                // Извлечение строки запроса
                if (pathAndQueryString.IndexOf('?')>=0)
                {
                    string[] pathAndQueryArray = pathAndQueryString.Split(new[] { '?' });
                    Path = pathAndQueryArray[0].Trim();
                    Query = pathAndQueryArray[1].Trim();
                }
                else
                {
                    Path = pathAndQueryString;
                    Query = "";
                }
                Version = first[2];
                // Получение заголовков
                int endOfHeaders = lines.IndexOf(""); // Тело отделяется от заголовков пустой строкой
                for (int i = 1; i < endOfHeaders; i++)
                {
                    string[] line = lines[i].Split(new[] { ':' });
                    for (int j = 2; j < line.Length; j++)
                    {
                        line[1] += ":" + line[j];
                    }
                    Headers.Add(line[0], line[1].Trim());
                }
                // Получение тела
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
        public string Query { get; set; }
        public string Version { get; set; }
        public Dictionary<string, string> Headers { get; set; } =  new Dictionary<string, string>();
        public string Body { get; set; }

        public override string ToString()
        {
            string result = Method + " " + Path + " " + Version + "\r\n";
            foreach (var item in Headers)
            {
                result += item.Key + ": " + item.Value + "\r\n";
            }
            result += "\r\n";
            result += Body;
            return result;
        }

        
    }
}
