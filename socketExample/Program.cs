using System;
using System.Collections.Generic; 
using System.Linq; 
using System.Text;  
using System.Threading.Tasks;
using System.Threading;
using System.Net;  
using System.Net.Sockets;
using System.IO;

namespace socketExample
{
    class Program
    {
        private static int _port = 5555;
        static  void Main(string[] args)
        {
            Server server = new Server(){port = _port};
            Task.Run(() => server.Run());
            Client client = new Client() { port = _port };
            client.Run();
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();

        }
    }

    class Server 
    {
         public int port
        {
            get;
            set;
        }

        public void Run()
        {
            try{
                TcpListener listener = new TcpListener(IPAddress.Any, port);
                listener.Start();
                Console.WriteLine("Server start listening");
                TcpClient clientConnected = listener.AcceptTcpClient();
                Console.WriteLine("Client connected");
                NetworkStream channel = clientConnected.GetStream();
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
                channel.Close();
                listener.Stop();

            }
            catch (SocketException e){
                Console.WriteLine("Socket error" + e.ToString());
            }
        }
    }


    class Client
    { 
        public int port
        {
            get;
            set;
        }
         public void Run(){
            try {
                TcpClient client = new TcpClient("localhost", port);
                using (NetworkStream channel = client.GetStream()){
                    BinaryReader reader = new BinaryReader(channel);
                    BinaryWriter writer = new BinaryWriter(channel);
                    string message = null;
                    Thread.Sleep(500);

                    do
                    {
                        Console.Write("Enter message (:Q to exit): ");
                        message = Console.ReadLine();

                        writer.Write(message);
                        writer.Flush();
                        message = reader.ReadString();
                        Console.WriteLine(message);
                    } while (!message.Contains(":Q"));
                }
                client.Close();
                client.Dispose();
            }
            catch (SocketException e)
            {
                Console.WriteLine("Socket error" + e.ToString());
            }

        }
    }
}
