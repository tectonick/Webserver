using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Timers;
using System.Diagnostics;
using System.Threading;

namespace Webserver
{
    class Server
    {
        public int Port { get; set; } = 8080;
        public IPAddress Address { get; set; } = IPAddress.Parse("127.0.0.1");
        public string WebRoot { get; set; } = "/";
        public string PHPFile { get; set; } = "";

        public delegate void Loggger(string data);
        public Loggger Log;
        public Server(IPAddress addr, int port, string root)
        {
            Port = port;
            Address = addr;
            WebRoot = root;
        }
        public Server() { }
        public void Start()
        {
            try
            {
                _server = new TcpListener(Address, Port);
                _server.Start();
                Log($"Server started on {Address}:{Port} with root at {WebRoot}");

                //Listening loop
                while (true)
                {
                    if (_server.Pending())
                    {
                        ThreadStart threadStart = new ThreadStart(this.ServeClient);
                        Thread clientThread = new Thread(threadStart);
                        clientThread.Start();
                    }
                }
            }
            catch (SocketException e)
            {
                Log($"SocketException: {e}");
                Console.WriteLine();
            }
            catch (ThreadAbortException e)
            {
                Log("Server thread aborted");
            }
            catch (Exception e)
            {

            }
            finally
            {
                Log("Server stopped");
                this.Stop();
            }
        }

        public void Stop()
        {
            _server.Stop();
        }


        private readonly int BUFFER_SIZE = 8192;
        private readonly List<string> DEFAULTFILES = new List<string>{ Path.Combine(".","index.html"), Path.Combine(".","index.htm"), Path.Combine(".","index.php"),"" };
        private readonly List<string> DEFAULTEXTENSIONS = new List<string> { ".html", ".htm", ".php", "" };
        private TcpListener _server = null;

        private string HandlePost(string pathToFile, string query)
        {
            ProcessStartInfo StartInfo = new ProcessStartInfo
            {
                FileName = PHPFile,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true,
                RedirectStandardInput = true
            };
            StartInfo.EnvironmentVariables.Add("REQUEST_METHOD", "POST");
            StartInfo.EnvironmentVariables.Add("CONTENT_LENGTH", "1234");
            StartInfo.EnvironmentVariables.Add("SCRIPT_FILENAME", pathToFile);
            StartInfo.EnvironmentVariables.Add("REDIRECT_STATUS", "CGI");
            StartInfo.EnvironmentVariables.Add("CONTENT_TYPE", "application/x-www-form-urlencoded");
            Process proc = new Process();
            proc.StartInfo = StartInfo;
            string outputPHP = "";
            proc.Start();
            var streamWriter = proc.StandardInput;
            streamWriter.WriteLine(query);
            streamWriter.WriteLine("");
            streamWriter.Close();
            while (!proc.StandardOutput.EndOfStream)
            {
                outputPHP += proc.StandardOutput.ReadLine();
            }
            outputPHP = outputPHP.Substring(outputPHP.IndexOf('<'));
            return outputPHP;            
        }

        private string HandleGet(string pathToFile, string query)
        {
            ProcessStartInfo StartInfo = new ProcessStartInfo
            {
                FileName = PHPFile,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true,
                RedirectStandardInput = true
            };
            StartInfo.EnvironmentVariables.Add("REQUEST_METHOD", "GET");
            StartInfo.EnvironmentVariables.Add("QUERY_STRING", query);
            StartInfo.EnvironmentVariables.Add("SCRIPT_FILENAME", pathToFile);
            StartInfo.EnvironmentVariables.Add("REDIRECT_STATUS", "0");
            StartInfo.EnvironmentVariables.Add("CONTENT_TYPE", "application/x-www-form-urlencoded");
            Process proc = new Process();
            proc.StartInfo = StartInfo;
            string outputPHP = "";
            proc.Start();
            while (!proc.StandardOutput.EndOfStream)
            {
                outputPHP += proc.StandardOutput.ReadLine();
            }
            return outputPHP;
        }

        private string CheckAndParseFilename(string requestPath)
        {
            string pathToFile = requestPath;
            string opt = "";
            pathToFile = pathToFile.Substring(1, pathToFile.Length - 1);
            bool isExtensionOmitted = false;
            if (!File.Exists(Path.Combine(WebRoot, pathToFile)))
            {
                //Try interpret path as folder name
                foreach (var item in DEFAULTFILES)
                {
                    opt = item;
                    if (File.Exists(Path.Combine(WebRoot, pathToFile, opt))) break;
                }
                //Try interpret path as filename with omitted extension
                if (opt == "")
                {
                    foreach (var item in DEFAULTEXTENSIONS)
                    {
                        opt = item;
                        if (File.Exists(Path.Combine(WebRoot, pathToFile + opt)))
                        {
                            isExtensionOmitted = true;
                            break;
                        }
                    }
                }
            }
            return (isExtensionOmitted) ? Path.Combine(WebRoot, pathToFile + opt) : Path.Combine(WebRoot, pathToFile, opt);
        }

        private void handlePHP(HTTPRequest request, HTTPResponse response, string pathToFile) {
            string phpOutput = "";
            if (PHPFile == "" || (!File.Exists(PHPFile)))
            {
                throw new FileNotFoundException();
            }
            if (request.Method == "POST")
            {
                phpOutput = HandlePost(pathToFile, request.Body);
            }
            else
            {
                phpOutput = HandleGet(pathToFile, request.Query);
            }

            string headers = "";
            int beginOfBody = phpOutput.IndexOf('<');
            if (beginOfBody >= 0)
            {
                response.Body = Encoding.ASCII.GetBytes(phpOutput.Substring(beginOfBody));
                headers = phpOutput.Substring(0, beginOfBody);
            }
            else
            {
                headers = phpOutput;
            }
            if (headers.IndexOf("Status: 302") >= 0)
            {
                int locationIndex = headers.IndexOf("location:") + "location:".Length;
                int endOfLocationIndex = headers.IndexOf("Content-type:");
                string location = headers.Substring(locationIndex, endOfLocationIndex - locationIndex);
                response.Headers.Add("location", location);
                response.StatusCode = "302";
            }
           
            response.Headers.Add("X-Powered-By", "PHP");
            response.Headers.Add("Content-Type", "text/html");
        }
        private void handleJPG(HTTPRequest request, HTTPResponse response, string pathToFile) {
            response.Body = File.ReadAllBytes(pathToFile);
            response.Headers.Add("Content-Type", "image/jpg");
            response.Headers.Add("Content-Length", response.Body.Length.ToString());
            response.isBodyBinary = true;
        }

        private void handleText(HTTPRequest request, HTTPResponse response, string pathToFile)
        {
            string extension =Path.GetExtension(pathToFile);

            switch (extension)
            {
                case ".html":
                    response.Headers.Add("Content-Type", "text/html");
                    break;
                case ".htm":
                    response.Headers.Add("Content-Type", "text/html");
                    break;
                case ".css":
                    response.Headers.Add("Content-Type", "text/css");
                    break;
                case ".js":
                    response.Headers.Add("Content-Type", "application/x-javascript");
                    break;
                default:
                    break;
            }

            response.Body=File.ReadAllBytes(pathToFile);
        }

        private void ServeClient()
        {
            TcpClient client;
            try
            {
                client = _server.AcceptTcpClient();
            }
            catch (Exception)
            {
                return;
            }
            Byte[] buffer = new Byte[BUFFER_SIZE];
            NetworkStream stream = client.GetStream();
            HTTPResponse response = new HTTPResponse();
            response.StatusCode = "200";
            try
            {
                //Get client request
                int requestLength = stream.Read(buffer, 0, buffer.Length);
                string rawRequest = System.Text.Encoding.ASCII.GetString(buffer, 0, requestLength);
                Log($"[{Thread.CurrentThread.ManagedThreadId}] Server received:\r\n{rawRequest}");
                
                //Parse request
                HTTPRequest request = new HTTPRequest(rawRequest);
                //Check filename and fill missing extensions or names
                string pathToFile = CheckAndParseFilename(request.Path);
                // Handle filetypes
                if (pathToFile.IndexOf(".php") >= 0)
                {
                    handlePHP(request, response, pathToFile);
                } else
                if (pathToFile.IndexOf(".jpg") >= 0)
                {
                    handleJPG(request, response, pathToFile);
                } else
                {
                    handleText(request, response, pathToFile);
                }
            }
            catch (ArgumentException)
            {
                response.StatusCode = "400";
            }
            catch (FileNotFoundException)
            {
                response.StatusCode = "404";
            }
            catch (Exception)
            {
                response.StatusCode = "500";
            }
            response.Headers.Add("Host", $"{Address.ToString()}:{Port}");
            response.Headers.Add("Server", "simple");
            response.Headers.Add("Date", DateTime.UtcNow.ToString("ddd, dd MMM yyy HH:mm:ss G'M'T"));
            byte[] readyResponse = response.ToBinary();
            stream.Write(readyResponse, 0, readyResponse.Length);
            stream.Close();

            Log($"[{Thread.CurrentThread.ManagedThreadId}] Server sent:\r\n{response.ToString()}");
        }


    }
}
