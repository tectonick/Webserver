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
        int requestNumber = 0;
        List<string> defaultFiles = new List<string>{ Path.Combine(".","index.html"), Path.Combine(".","index.htm"), Path.Combine(".","index.php"),"" };
        List<string> defaultExtensions = new List<string> { ".html", ".htm", ".php", "" };
        private TcpListener _server = null;
        public int Port { get; set; } = 8080;
        public IPAddress Address { get; set; } = IPAddress.Parse("127.0.0.1");
        public string WebRoot { get; set; } = "/";
        public string PHPFile { get; set; } = "";
        public Server(IPAddress addr, int port, string root)
        {
            Port = port;
            Address = addr;
            WebRoot = root;
        }
        public Server()
        {
        }

        string HandlePost(string pathToFile, string query)
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


        string HandleGet(string pathToFile, string query)
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
            //var streamWriter = proc.StandardInput;
            //streamWriter.WriteLine(query);
            //streamWriter.WriteLine("");
            //streamWriter.Close();

            while (!proc.StandardOutput.EndOfStream)
            {
                outputPHP += proc.StandardOutput.ReadLine();
            }
            outputPHP = outputPHP.Substring(outputPHP.IndexOf('<'));
            return outputPHP;

        }


        void ServeClient()
        {
            TcpClient client = _server.AcceptTcpClient();
            Byte[] buffer = new Byte[4096];
            String data = null;
            data = null;
            NetworkStream stream = client.GetStream();
            int requestLength;
            HTTPResponse response = new HTTPResponse();
            string delimiter = "______________________________________________________";
            try
            {
                requestLength = stream.Read(buffer, 0, buffer.Length);

                data = System.Text.Encoding.ASCII.GetString(buffer, 0, requestLength);
                
                Console.WriteLine("{2}\r\n_{0}_ Received: {1}\r\n{2}", requestNumber, data,delimiter);

                //Parse request
                HTTPRequest request = new HTTPRequest(data);

                //TODO parse file types

                string pathToFile = request.Path;
                string opt = "";
                pathToFile = pathToFile.Substring(1, pathToFile.Length - 1);


                bool isExtensionOmitted = false;
                if (!File.Exists(Path.Combine(WebRoot, pathToFile)))
                {
                    //Try interpret path as folder name
                    foreach (var item in defaultFiles)
                    {
                        opt = item;
                        if (File.Exists(Path.Combine(WebRoot, pathToFile,opt))) break;
                    }
                    //Try interpret path as filename with omitted extension
                    if (opt == "")
                    {
                        foreach (var item in defaultExtensions)
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

                pathToFile = (isExtensionOmitted) ? Path.Combine(WebRoot, pathToFile + opt) : Path.Combine(WebRoot, pathToFile, opt);

                byte[] bodyData = File.ReadAllBytes(pathToFile);


                if (pathToFile.IndexOf(".php") >= 0)
                {
                    if (PHPFile==""||(!File.Exists(PHPFile)))
                    {
                        throw new FileNotFoundException();
                    }
                    if (request.Method=="POST")
                    {
                        bodyData = Encoding.ASCII.GetBytes(HandlePost(pathToFile, request.Body));
                    }
                    else
                    {
                        bodyData = Encoding.ASCII.GetBytes(HandleGet(pathToFile, request.Query));
                    }
                    
                }

                //TODO Move to another class and add type parsing
                if (pathToFile.IndexOf(".jpg") >= 0)
                {
                    response.Headers.Add("Content-Type", "image/jpg");
                    response.Headers.Add("Content-Length", bodyData.Length.ToString());
                    response.isBodyBinary = true;
                }


                response.Body = bodyData;
                response.StatusCode = "200";

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
            response.Headers.Add("Host", "localhost:8080");
            data = response.ToString();

            byte[] byteData = response.ToBinary();
            stream.Write(byteData, 0, byteData.Length);
            Console.WriteLine("{2}\r\n_{0}_ Sent: \n{1} \r\n{2}", requestNumber, data, delimiter);
            stream.Close();
            requestNumber++;
        }

        public void Start()
        {            
            try
            {
                _server = new TcpListener(Address, Port);
                _server.Start();
                Console.WriteLine($"Server started on {Address}:{Port} with root at {WebRoot}");
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
                Console.WriteLine("SocketException: {0}", e);
            }
            catch (ThreadAbortException e)
            {
                Console.WriteLine("Server thread aborted");                
            }
            finally
            {
                Console.WriteLine("Server stopped");
                this.Stop();
            }
        }

        public void Stop()
        {
            _server.Stop();
        }
    }
    
}
