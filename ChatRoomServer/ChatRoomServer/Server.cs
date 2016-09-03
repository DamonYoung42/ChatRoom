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

        public static Dictionary<string, TcpClient> chatUsers = new Dictionary<string, TcpClient>();
        public static Queue<string> messageQueue = new Queue<string>();
        public static BinaryTree userTree = new BinaryTree();

        public void RunServer()
        {
            
            try
            {     
                TcpListener serverSocket = new TcpListener(IPAddress.Any, 10000);
                //TcpClient clientSocket = default(TcpClient);

                serverSocket.Start();
                Console.WriteLine("Chat Server Initiated ....");


                Thread monitorConnections = new Thread(MonitorConnections);
                monitorConnections.Start();
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
                Console.WriteLine("Chat Server is shutting down.");
                Broadcast("System", "Chat Server shutting down.");
                Console.ReadLine();
            }

        }

        public void InitializeClient(TcpClient clientSocket)
        {
            try
            {
                byte[] bytesFrom = new byte[4096];
                byte[] bytesSent = new byte[4096];
                string userInput = "";
                NetworkStream networkStream = clientSocket.GetStream();
                bytesSent = Encoding.ASCII.GetBytes("Welcome to the Chat Server.\nYour first message will be used as your name.\nEnter 'exit' to close the application.");
                networkStream.Write(bytesSent, 0, bytesSent.Length);
                networkStream.Read(bytesFrom, 0, bytesFrom.Length);
                userInput = Encoding.ASCII.GetString(bytesFrom);
                userInput = userInput.Substring(0, userInput.IndexOf("\0"));
                if (userInput != "exit")
                {
                    while (chatUsers.ContainsKey(userInput))
                    {
                        bytesSent = Encoding.ASCII.GetBytes("Someone with that name is already logged on. Please enter a different name.");
                        networkStream.Write(bytesSent, 0, bytesSent.Length);
                        networkStream.Read(bytesFrom, 0, bytesFrom.Length);
                        userInput = Encoding.ASCII.GetString(bytesFrom);
                        userInput = userInput.Substring(0, userInput.IndexOf("\0"));
                    }
                    MonitorClient client = new MonitorClient();
                    client.startClient(clientSocket, userInput);
                    userTree.Insert(userInput, clientSocket);
                    chatUsers.Add(userInput, clientSocket);

                    Console.WriteLine(userInput + " has joined the chat room. ");

                    bytesSent = Encoding.ASCII.GetBytes("Hello, " + userInput);
                    networkStream.Write(bytesSent, 0, bytesSent.Length);
                    messageQueue.Enqueue(userInput + " has joined the chat room.");
                    Broadcast(userInput, userInput + " has joined the chat room.");
                }


                //BroadcastMessageQueue();}
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
            }

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
                        bytesOut = Encoding.ASCII.GetBytes(message);
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
                //        bytesOut = Encoding.ASCII.GetBytes(message);
                //        NetworkStream broadcast = node.tcpClient.GetStream();
                //        broadcast.Write(bytesOut, 0, bytesOut.Length);
                //        broadcast.Flush();
                //    }
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

        private void MonitorConnections()
        {
            while (true)
            {


                try
                {
                    if (userTree.Count() > 0)
                    {
                        foreach (Node node in userTree)
                        {
                            if (!node.tcpClient.Connected)
                            {
                                // string name = node.name;
                                chatUsers.Remove(node.name);
                                Broadcast("Server", node.name + " has been disconnected.");
                            }
                        }

                    }
                    userTree = new BinaryTree();

                    foreach (KeyValuePair<string, TcpClient> user in chatUsers)
                    {
                        userTree.Insert(user.Key, user.Value);
                    }
                    //foreach (KeyValuePair<string, TcpClient> user in chatUsers)
                    //{
                    //    if (!user.Value.Connected)
                    //    {
                    //        string name = user.Key;
                    //        //chatUsers.Remove(user.Key);
                    //        userTree.Delete(name);
                    //        Broadcast("Server", name + " has been disconnected.");
                    //    }

                    //}


                }
                catch (Exception error)
                {
                    Console.WriteLine(error);
                }
            }

        }
    }



}

