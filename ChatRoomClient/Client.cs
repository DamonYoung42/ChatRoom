using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;


namespace ChatRoomClient
{
    class Client
    {
        public void RunClient()
        {
            try
            {

                TcpClient client = new TcpClient("localhost", 1000);
                Console.WriteLine("You are connected to the message server.");

                NetworkStream networkStream = client.GetStream();

                byte[] message = new byte[1024];

                int messageLength = networkStream.Read(message, 0, message.Length);

                Console.WriteLine(Encoding.ASCII.GetString(message, 0, messageLength));

                client.Close();

            }

            catch (Exception error)
            {
                Console.WriteLine(error.ToString());
            }
            //TcpClient server;
            //bool connected = true;
            //try
            //{
            //    server = new TcpClient("localhost", 1000);
            //    Console.WriteLine("Connected to chat server");
            //}
            //catch
            //{
            //    Console.WriteLine("Could not connect to chat server");
            //    return;
            //}

            //NetworkStream networkStream = server.GetStream();
            //StreamReader streamreader = new StreamReader(networkStream);
            //StreamWriter streamwriter = new StreamWriter(networkStream);

            //try
            //{
            //    string clientMessage = "";
            //    string serverMessage = "";
            //    while (connected)
            //    {
            //        Console.Write("Client:");
            //        clientMessage = Console.ReadLine();
            //        if (clientMessage.ToLower() == "goodbye")
            //        {
            //            connected = false;
            //            streamwriter.WriteLine("goodbye");
            //            streamwriter.Flush();
            //        }
            //        else 
            //        {
            //            streamwriter.WriteLine(clientMessage);
            //            streamwriter.Flush();
            //            serverMessage = streamreader.ReadLine();
            //            Console.WriteLine("Server:" + serverMessage);
            //        }
            //    }
            //}
            //catch
            //{
            //    Console.WriteLine("Could not read message from server.");
            //}
            //streamreader.Close();
            //networkStream.Close();
            //streamwriter.Close();
        }
    }
}
