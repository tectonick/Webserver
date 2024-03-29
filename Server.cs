﻿using System;
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
        // Конфигурационные свойства
        public int Port { get; set; } = 8080;
        public IPAddress Address { get; set; } = IPAddress.Parse("127.0.0.1");
        public string WebRoot { get; set; } = "/";
        public string PHPFile { get; set; } = "";

        // Ссылка на функцию логирования
        public delegate void Loggger(string data);
        public Loggger Log;
        // Событие остановки сервера
        public delegate void Stopped();
        public event Stopped OnStop;
        public Server(IPAddress addr, int port, string root)
        {
            Port = port;
            Address = addr;
            WebRoot = root;
        }
        public Server() { }

        //Метод запуска сервера
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
                //Log("Server thread aborted");
            }
            catch (Exception e)
            {
            }
            finally
            {
                Log("Server stopped");
                OnStop.Invoke();
                this.Stop();
            }
        }

        // Метод остановки сервера
        public void Stop()
        {
            _server.Stop();
        }


        private readonly int BUFFER_SIZE = 8192;
        private readonly List<string> DEFAULTFILES = new List<string>{ Path.Combine(".","index.html"), Path.Combine(".","index.htm"), Path.Combine(".","index.php"),"" };
        private readonly List<string> DEFAULTEXTENSIONS = new List<string> { ".html", ".htm", ".php", "" };
        private TcpListener _server = null;

        // Метод запуска интерпретатора php с параметрами для обработки POST запроса
        private string HandlePost(string pathToFile, string query)
        {
            // Установка параметров
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

            // Запись запроса и чтение результата
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

        // Метод запуска интерпретатора php с параметрами для обработки GET запроса
        private string HandleGet(string pathToFile, string query)
        {
            // Установка параметров
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

            // Чтение результата
            while (!proc.StandardOutput.EndOfStream)
            {
                outputPHP += proc.StandardOutput.ReadLine();
            }
            return outputPHP;
        }
        //Метод для проверки пути к файлу и добавления опущенных частей пути при необходимости
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

        // Метод для чтения динамического файла в ответ с пропуском через интерпретатор php
        private void handlePHP(HTTPRequest request, HTTPResponse response, string pathToFile) {
            string phpOutput = "";

            //Проверка наличия php по заданному пути
            if (PHPFile == "" || (!File.Exists(PHPFile)))
            {
                throw new FileNotFoundException();
            }
            // Определение метода запроса
            if (request.Method == "POST")
            {
                phpOutput = HandlePost(pathToFile, request.Body);
            }
            else
            {
                phpOutput = HandleGet(pathToFile, request.Query);
            }

            // Отсоединение от телда заголовков, добавленных интерпретатором php
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
            // Проверка на предмет перенаправления
            if (headers.IndexOf("Status: 302") >= 0)
            {
                int locationIndex = headers.IndexOf("location:") + "location:".Length;
                int endOfLocationIndex = headers.IndexOf("Content-type:");
                string location = headers.Substring(locationIndex, endOfLocationIndex - locationIndex);
                response.Headers.Add("location", location);
                response.StatusCode = "302";
            }
           
            response.Headers.Add("X-Powered-By", "PHP");
        }

        // Метод для чтения статического файла в ответ
        private void handleStatic(HTTPRequest request, HTTPResponse response, string pathToFile)
        { 
            response.Body=File.ReadAllBytes(pathToFile);
        }

        private void AddContentTypeHeader(HTTPResponse response, string pathToFile)
        {
            string extension = Path.GetExtension(pathToFile);

            switch (extension)
            {
                case ".html":
                case ".htm":
                case ".php":
                    response.Headers.Add("Content-Type", "text/html");
                    break;
                case ".css":
                    response.Headers.Add("Content-Type", "text/css");
                    break;
                case ".js":
                    response.Headers.Add("Content-Type", "text/javascript");
                    break;
                case ".jpg":
                case ".jpeg":
                case ".jfif":
                case ".pjpeg":
                case ".pjp":
                    response.Headers.Add("Content-Type", "image/jpeg");
                    response.isBodyBinary = true;
                    break;
                case ".ico":
                case ".cur":
                    response.Headers.Add("Content-Type", "image/x-icon");
                    response.isBodyBinary = true;
                    break;
                case ".png":
                    response.Headers.Add("Content-Type", "image/png");
                    response.isBodyBinary = true;
                    break;
                case ".apng":
                    response.Headers.Add("Content-Type", "image/apng");
                    response.isBodyBinary = true;
                    break;
                case ".gif":
                    response.Headers.Add("Content-Type", "image/gif");
                    response.isBodyBinary = true;
                    break;
                case ".bmp":
                    response.Headers.Add("Content-Type", "image/bmp");
                    response.isBodyBinary = true;
                    break;
                case ".svg":
                    response.Headers.Add("Content-Type", "image/svg+xml");
                    response.isBodyBinary = true;
                    break;
                case ".webp":
                    response.Headers.Add("Content-Type", "image/webp");
                    response.isBodyBinary = true;
                    break;
                case ".tiff":
                case ".tif":
                    response.Headers.Add("Content-Type", "image/tiff");
                    response.isBodyBinary = true;
                    break;
                default:
                    response.Headers.Add("Content-Type", "text/plain");                    
                    break;
            }
        }

        private void ServeClient()
        {
            //Инициализация и прием клиента
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
                //Получение запроса
                int requestLength = stream.Read(buffer, 0, buffer.Length);
                string rawRequest = System.Text.Encoding.ASCII.GetString(buffer, 0, requestLength);
                Log($"[{Thread.CurrentThread.ManagedThreadId}] Server received:\r\n{rawRequest}");
                
                //Парсинг запроса
                HTTPRequest request = new HTTPRequest(rawRequest);
                if (request.Version != "HTTP/1.1")
                {
                    throw new NotSupportedException();
                }
                if ((request.Method!="POST")&&(request.Method != "GET"))
                {
                    throw new InvalidOperationException();
                }

                //Проверить путь к файлу и добавить опущенные части
                string pathToFile = CheckAndParseFilename(request.Path);

                // Handle filetypes
                if (pathToFile.IndexOf(".php") >= 0)
                {
                    handlePHP(request, response, pathToFile);
                } else
                {
                    handleStatic(request, response, pathToFile);
                }

                //Добавить MIME тип в заголовк Content-type
                AddContentTypeHeader(response, pathToFile);

                response.Headers.Add("Content-Length", response.Body.Length.ToString());
            }
            catch (NotSupportedException)
            {
                response.StatusCode = "405";
            }
            catch (InvalidOperationException)
            {
                response.StatusCode = "405";
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
            // Добавление заголовков
            response.Headers.Add("Host", $"{Address.ToString()}:{Port}");
            response.Headers.Add("Server", "simple");
            response.Headers.Add("Date", DateTime.UtcNow.ToString("ddd, dd MMM yyy HH:mm:ss G'M'T"));
            // Передача ответа клиенту
            byte[] readyResponse = response.ToBinary();
            stream.Write(readyResponse, 0, readyResponse.Length);
            stream.Close();

            Log($"[{Thread.CurrentThread.ManagedThreadId}] Server sent:\r\n{response.ToString()}");
        }


    }
}
