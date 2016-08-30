using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;


namespace ChatRoomServer
{
    class Server
    {

        public void RunServer()
        {
            bool waiting = true;

            TcpListener tcpListener = new TcpListener(IPAddress.Any, 1000);

            tcpListener.Start();

            while (waiting)
            {
                Console.WriteLine("Awaiting connection ...");
                TcpClient client = tcpListener.AcceptTcpClient();

                Console.WriteLine("Connection made.");
                NetworkStream networkStream = client.GetStream();


            }
            //try
            //{
            //    bool waiting = true;
            //    string serverMessage = "";
            //    string clientMessage = "";
            //    TcpListener tcpListener = new TcpListener(IPAddress.Parse("127.0.0.1"), 1000);
            //    tcpListener.Start();
            //    Console.WriteLine("Server Started");
            //    Socket client = tcpListener.AcceptSocket();
            //    Console.WriteLine("Client Connected");
            //    NetworkStream networkStream = new NetworkStream(client);
            //    StreamWriter streamwriter = new StreamWriter(networkStream);
            //    StreamReader streamreader = new StreamReader(networkStream);
            //    while (waiting)
            //    {
            //        if (client.Connected)
            //        {
            //            clientMessage = streamreader.ReadLine();
            //            Console.WriteLine("Client:" + clientMessage);
            //            if ((clientMessage.ToLower() == "goodbye"))
            //            {
            //                waiting = false;
            //                streamreader.Close();
            //                streamwriter.Close();
            //                networkStream.Close();
            //                return;
            //            }
            //            Console.Write("Server:");
            //            serverMessage = Console.ReadLine();
            //            streamwriter.WriteLine(serverMessage);
            //            streamwriter.Flush();
            //        }
            //    }
            //    streamreader.Close();
            //    networkStream.Close();
            //    streamwriter.Close();
            //    client.Close();
            //    Console.WriteLine("Shutting down.");

            //}
            //catch (Exception error)
            //{
            //    Console.WriteLine(error.ToString());
            //}
        }
    }
}

