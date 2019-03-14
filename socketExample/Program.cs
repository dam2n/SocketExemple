using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace socketExample
{
    class Program
    {
       
        static  void Main(string[] args)
        {
            Server server = new Server();
            server.Run();
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();

        }
    }

    class Server
    {
        public int Port { get; }

        public Server() => Port = 23000;
        public Server(int port)
        {
            if (port > 1024 && port < 65536)
                Port = port;
            else
                Port = 23000;
        }


        public void Run()
        {

            var listener = new TcpListener(IPAddress.Any, Port);

            try
            {
                listener.Start();
                Console.WriteLine("Server start listening");
                TcpClient clientConnected = listener.AcceptTcpClient();
                Console.WriteLine("Client connected");
                using (NetworkStream channel = clientConnected.GetStream())
                {
                    BinaryReader reader = new BinaryReader(channel);
                    BinaryWriter writer = new BinaryWriter(channel);
                    string message = null;
                    do
                    {
                        message = reader.ReadString();
                        writer.Write("Server >> you wrote " + message);
                        writer.Flush();
                    } while (message != ":Q");
                    Console.WriteLine("Client desconnected");
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("Socket error" + e.ToString());
            }

            finally
            {

                listener.Stop();
            }


        }



    }

}
