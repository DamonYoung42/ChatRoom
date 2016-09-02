using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace ChatRoomServer
{
    public class MonitorClient
    {
        TcpClient clientSocket;
        string userName;
        Dictionary<string, TcpClient> clientsList;

        public void startClient(TcpClient ClientSocket, string userName, Dictionary<string, TcpClient> cList)
        {
            this.clientSocket = ClientSocket;
            this.userName = userName;
            this.clientsList = cList;
            Thread clientThread = new Thread(Communicate);
            clientThread.Start();
        }

        private void Communicate()
        {

            try
            {
                while (true)
                {
                    byte[] bytesFrom = new byte[4096];
                    string dataFromClient = null;
                    NetworkStream networkStream = clientSocket.GetStream();
                    networkStream.Read(bytesFrom, 0, bytesFrom.Length);

                    dataFromClient = Encoding.ASCII.GetString(bytesFrom);

                    dataFromClient = dataFromClient.Substring(0, dataFromClient.IndexOf("\0"));

                    

                    if (dataFromClient.ToLower() == "exit")
                    {
                        dataFromClient = userName + " has left the chat room.";
                        Server.chatUsers.Remove(this.userName);
                    }
                    {
                        dataFromClient = "<" + this.userName + ">" + dataFromClient;
                    }

                    Console.WriteLine(dataFromClient);

                    Server.Broadcast(dataFromClient);
                   
                }

            }
                        
            catch (Exception error)
            {
                Console.WriteLine(error);
            }
        }
    }
}
