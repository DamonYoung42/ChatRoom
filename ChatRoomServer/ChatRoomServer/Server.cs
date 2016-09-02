using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;


namespace ChatRoomServer
{
    public class Server
    {
        public static BinaryTree userTree = new BinaryTree();
        public static Dictionary<string, TcpClient> chatUsers = new Dictionary<string, TcpClient>();
        public static Queue<string> messageQueue = new Queue<string>();

        public void RunServer()
        {
            
            try
            {     
                TcpListener serverSocket = new TcpListener(IPAddress.Any, 10000);
                //TcpClient clientSocket = default(TcpClient);

                serverSocket.Start();
                Console.WriteLine("Chat Server Initiated ....");
                
                while (true)
                {
                    //Thread broadcastMessages = new Thread(() => BroadcastMessageQueue());
                    //broadcastMessages.Start();
                    TcpClient clientSocket = serverSocket.AcceptTcpClient();
                    Thread clientThread = new Thread(() => InitializeClient(clientSocket));
                    clientThread.Start();
                }
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
            }
            finally
            {
                Console.WriteLine("Chat Server shutting down.");
                Console.ReadLine();
            }

        }

        public void InitializeClient(TcpClient clientSocket)
        {
            byte[] bytesFrom = new byte[4096];
            byte[] bytesSent = new byte[4096];
            string userInput = null;
            NetworkStream networkStream = clientSocket.GetStream();
            bytesSent = Encoding.ASCII.GetBytes("Welcome to the Chat Server. Your first message will be used as your name.");
            networkStream.Write(bytesSent, 0, bytesSent.Length);
            networkStream.Read(bytesFrom, 0, bytesFrom.Length);
            userInput = Encoding.ASCII.GetString(bytesFrom);
            userInput = userInput.Substring(0, userInput.IndexOf("\0"));
            MonitorClient client = new MonitorClient();
            client.startClient(clientSocket, userInput);
            userTree.Insert(userInput, clientSocket);
            chatUsers.Add(userInput, clientSocket);

            Console.WriteLine(userInput + " joined the chat room. ");

            bytesSent = Encoding.ASCII.GetBytes("Hello, " + userInput);
            networkStream.Write(bytesSent, 0, bytesSent.Length);
            messageQueue.Enqueue(userInput + " joined the chat room.");
            Broadcast(userInput, userInput + " joined the chat room.");
            //BroadcastMessageQueue();
        }

        public static void Broadcast(string userName, string message)
        {
            try
            {                              
                foreach (KeyValuePair<string, TcpClient> user in chatUsers)
                {
                    if (userName != user.Key)
                    {
                        byte[] bytesOut = null;
                        bytesOut = System.Text.Encoding.ASCII.GetBytes(message);
                        NetworkStream broadcast = user.Value.GetStream();
                        broadcast.Write(bytesOut, 0, bytesOut.Length);
                        broadcast.Flush();
                    }

                }

                //foreach (Node node in userTree)
                //{
                //    if (userName != node.name)
                //    {
                //        byte[] bytesOut = null;
                //        bytesOut = System.Text.Encoding.ASCII.GetBytes(message);
                //        NetworkStream broadcast = node.tcpClient.GetStream();
                //        broadcast.Write(bytesOut, 0, bytesOut.Length);
                //        broadcast.Flush();
                //    }

                //}
                //bytesOut = null;
                //}
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
            }

        }

        public void BroadcastMessageQueue()
        {
            while (messageQueue.Count > 0)
            {
                byte[] bytesOut = System.Text.Encoding.ASCII.GetBytes(messageQueue.Dequeue());

                foreach (KeyValuePair<string, TcpClient> user in chatUsers)
                {
                    //byte[] bytesOut = null;
                    //bytesOut = System.Text.Encoding.ASCII.GetBytes(message);
                    NetworkStream broadcast = user.Value.GetStream();
                    broadcast.Write(bytesOut, 0, bytesOut.Length);
                    broadcast.Flush();
                    //bytesOut = null;
                }
            }
        }
    }



}

