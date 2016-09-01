using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;


namespace ChatRoomServer
{
    public class Server
    {
        public static Dictionary<string, TcpClient> chatUsers = new Dictionary<string, TcpClient>();
        public static Queue<string> messageQueue = new Queue<string>();

        public void RunServer()
        {
            try
            {     
                TcpListener serverSocket = new TcpListener(IPAddress.Any, 10000);
                TcpClient clientSocket = default(TcpClient);

                serverSocket.Start();
                Console.WriteLine("Chat Server Started ....");

                while (true)
                {
                    clientSocket = serverSocket.AcceptTcpClient();

                    byte[] bytesFrom = new byte[4096];
                    byte[] bytesSent = new byte[4096];
                    string dataFromClient = null;
                    NetworkStream networkStream = clientSocket.GetStream();
                   
                    networkStream.Read(bytesFrom, 0, bytesFrom.Length);
                    dataFromClient = Encoding.ASCII.GetString(bytesFrom);
                    dataFromClient = dataFromClient.Substring(0, dataFromClient.IndexOf("\0"));

                    Console.WriteLine(dataFromClient + " joined the chat room. ");
                    bytesSent = Encoding.ASCII.GetBytes("Welcome, " + dataFromClient);
                    networkStream.Write(bytesSent, 0, bytesSent.Length);
                    Broadcast(dataFromClient + " joined the chat room.");

                    chatUsers.Add(dataFromClient, clientSocket);
                    
                    MonitorClient client = new MonitorClient();
                    client.startClient(clientSocket, dataFromClient, chatUsers);
                }
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
            }
            finally
            {
                Console.WriteLine("exit");
                Console.ReadLine();
            }

        }

        public static void Broadcast(string message)
        {
            try
            {
                //while (messageQueue.Count > 0)
                //{
                    //byte[] bytesOut = System.Text.Encoding.ASCII.GetBytes(messageQueue.Dequeue());
                    foreach (KeyValuePair<string, TcpClient> user in chatUsers)
                    {
                    byte[] bytesOut = null;
                    bytesOut = System.Text.Encoding.ASCII.GetBytes(message);
                    NetworkStream broadcast = user.Value.GetStream();
                    broadcast.Write(bytesOut, 0, bytesOut.Length);
                    broadcast.Flush();
                    
                }
                //}
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
            }

        }
    }



}

