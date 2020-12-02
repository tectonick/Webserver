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

namespace Webserver
{
    class Server
    {

        private TcpListener _server = null;
        public int Port { get; set; } = 8080;
        public IPAddress Address { get; set; } = IPAddress.Parse("127.0.0.1");
        Byte[] bytes = new Byte[2048];
        public string WebRoot { get; set; } = "/";
        public Server(IPAddress addr, int port, string root)
        {
            Port = port;
            Address = addr;
            WebRoot = root;
        }
        public Server()
        {

        }
        public void Start()
        {            
            try
            {
                _server = new TcpListener(Address, Port);
                _server.Start();
                String data = null;
                Console.WriteLine($"Server started on {Address}:{Port} with root at {WebRoot}");
                int seq = 0;
                //Listening loop
                while (true)
                {
                    
                    //Console.Write("Waiting for a connection... ");
                    if (!_server.Pending())
                    {
                        //Console.WriteLine("Sorry, no connection requests have arrived");
                    }
                    else
                    {
                        TcpClient client = _server.AcceptTcpClient();
                            
                        //Console.WriteLine("Connected!");
                        data = null;
                        NetworkStream stream = client.GetStream();
                        int i;

                        i = stream.Read(bytes, 0, bytes.Length);

                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                        Console.WriteLine("_{0}_ Received: {1}", seq, data);

                        //Parse request
                        HTTPRequest request = new HTTPRequest(data);

                        string pathToFile = request.Path;
                        if (pathToFile[pathToFile.Length-1]=='/')
                        {
                            pathToFile += "index.html";
                        }
                        pathToFile = pathToFile.Substring(1, pathToFile.Length - 1);
                        string fileBody = File.ReadAllText(Path.Combine(WebRoot, pathToFile)); ///TEST

                        // TODO Fill response
                        HTTPResponse response = new HTTPResponse();
                        response.StatusCode = "200";
                        response.Headers.Add("Host", "localhost:8080");
                        response.Body = fileBody;

                        // Send back a response
                        data = response.ToString();
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);
                        stream.Write(msg, 0, msg.Length);
                        Console.WriteLine("_{0}_ Sent: \n{1}", seq, data);
                        seq++;
                  
                    } 
                        
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                this.Stop();
            }
        }


        public void Stop()
        {
            _server.Stop();
        }
    }
    
}
