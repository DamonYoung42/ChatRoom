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
    public class MonitorClient
    {
        TcpClient clientSocket;
        string userName;

        public void startClient(TcpClient ClientSocket, string userName)
        {
            this.clientSocket = ClientSocket;
            this.userName = userName;
            
            Thread clientThread = new Thread(Communicate);
            clientThread.Start();
        }

        private void Communicate()
        {

            try
            {
                while (clientSocket.Connected)
                {
                    byte[] bytesFrom = new byte[4096];
                    string userInput = null;
                    NetworkStream networkStream = clientSocket.GetStream();
                    networkStream.Read(bytesFrom, 0, bytesFrom.Length);

                    userInput = Encoding.ASCII.GetString(bytesFrom);

                    userInput = userInput.Substring(0, userInput.IndexOf("\0"));

                    if (userInput.ToLower() == "exit")
                    {
                        ProcessExitingClient(userName);
                        //userInput = userName + " has left the chat room.";
                        //Server.userTree.Delete(userName);
                        //Server.chatUsers.Remove(userName);
                        //Server.WriteMessageToServer(userInput);
                        ////Console.WriteLine(userInput);
                        //Server.messageQueue.Enqueue(userInput);
                        //Server.Broadcast(userName, userInput);
                        break;
                    }
                    else
                    {
                        userInput = "<" + userName + ">" + userInput;
                        //Console.WriteLine(userInput);
                        Server.WriteMessageToServer(userInput);
                        Server.messageQueue.Enqueue(userInput);
                        Server.Broadcast(userName, userInput);
                    }
                
                }

            }
                        
            catch (Exception error)
            {
                Server.WriteMessageToServer(error.ToString());
                //Console.WriteLine(error);
            }
            finally { 
                if (clientSocket != null)
                {
                    clientSocket.Close();
                }                   
            }
        }

        private void ProcessExitingClient(string userName)
        {
            string message = userName + " has left the chat room.";
            Server.userTree.Delete(userName);
            Server.chatUsers.Remove(userName);
            Server.WriteMessageToServer(message);
            //Console.WriteLine(userInput);
            Server.messageQueue.Enqueue(message);
            Server.Broadcast(userName, message);
        }
        
    }
}
