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
        Byte[] bytes = new Byte[4096];
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
                        HTTPResponse response = new HTTPResponse();
                        try
                        {
                            i = stream.Read(bytes, 0, bytes.Length);

                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                        Console.WriteLine("_{0}_ Received: {1}", seq, data);

                        //Parse request
                        HTTPRequest request = new HTTPRequest(data);

                       
                        //TODO parse file types

                            string pathToFile = request.Path;
                            if (pathToFile[pathToFile.Length - 1] == '/')
                            {
                                pathToFile += "index.html";
                            }

                            pathToFile = pathToFile.Substring(1, pathToFile.Length - 1);
                            byte[] bodyData= File.ReadAllBytes(Path.Combine(WebRoot, pathToFile));
                            

                            //TODO Move to another class and add type parsing
                            if (pathToFile.IndexOf(".jpg") >= 0)
                            {
                                response.Headers.Add("Content-Type", "image/jpg");
                                response.Headers.Add("Content-Length", bodyData.Length.ToString());                                
                            }


                            response.Body = bodyData;
                            response.StatusCode = "200";

                        }
                        catch(ArgumentException)
                        {
                            response.StatusCode = "400";
                        }
                        catch (FileNotFoundException)
                        {
                            response.StatusCode = "404";
                        }
                        catch(Exception)
                        {
                            response.StatusCode = "500";
                        }
                        response.Headers.Add("Host", "localhost:8080");

                        // TODO Fill response



                        // Send back a response
                        data = response.ToString();

                        //byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);
                        //stream.Write(msg, 0, msg.Length);

                        byte[] byteData = response.ToBinary();
                        stream.Write(byteData, 0, byteData.Length);
                        Console.WriteLine("_{0}_ Sent: \n{1}", seq, data);
                        stream.Close();
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
