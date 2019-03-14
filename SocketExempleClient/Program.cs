using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace SocketExempleClient
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            var client = new Client();
            client.Run();
            Console.ReadKey();
        }

        class Client
        {


            public string ServerAddress { get; }

            public int Port { get; }

            public Client() => (ServerAddress, Port) = ("localhost", 23000);


            public void Run()
            {

                    TcpClient client = new TcpClient(ServerAddress, Port);
                try { 
                    using (NetworkStream channel = client.GetStream())
                    {
                        BinaryReader reader = new BinaryReader(channel);
                        BinaryWriter writer = new BinaryWriter(channel);
                        string message = null;

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

                }
                catch (SocketException e)
                {
                    Console.WriteLine("Socket error" + e.ToString());
                }
                finally
                {
                    client.Close();
                    client.Dispose();
                }
            }
        }
    }
}




        

