using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.IO;
using Chat = System.Net;




namespace ChatRoomClient
{
    class Client
    {

        TcpClient clientStream = new TcpClient();
        NetworkStream serverStream = default(NetworkStream);

        public void RunClient()
        {
            try
            {
                //Console.WriteLine("Please enter your name before connecting to the Chat Server.");
                //string userName = Console.ReadLine();

                clientStream.Connect("localhost", 10000);
                //Console.WriteLine("Connected to Chat Server ...");
                //byte[] outStream = System.Text.Encoding.ASCII.GetBytes(userName);
                //serverStream = clientStream.GetStream();
                //serverStream.Write(outStream, 0, outStream.Length);
                //serverStream.Flush();

                while (clientStream.Connected)
                {
                    Thread receiveMessage = new Thread(ReceiveMessage);
                    receiveMessage.Start();
                    serverStream = clientStream.GetStream();
                    string message = Console.ReadLine();

                    byte[] outMessage = Encoding.ASCII.GetBytes(message);
                    serverStream.Write(outMessage, 0, outMessage.Length);
                    serverStream.Flush();

                    if (message.ToLower() == "exit")
                    {
                        Environment.Exit(0);
                    }


                }
            }
            catch (Exception)
            {
                Console.WriteLine("You are currently unable to establish a connection to the server.");
                Console.WriteLine("Please exit and restart client after confirming your chat server is running.");
                Console.ReadLine();


            }
            finally
            {
                if (clientStream != null)
                {
                    clientStream.Close();
                }
            }
        }


        private void ReceiveMessage()
        {
            while (true)
            {
                byte[] bytesFrom = new byte[4096];
                serverStream.Read(bytesFrom, 0, bytesFrom.Length);
                string message = Encoding.ASCII.GetString(bytesFrom);
                message = message.Substring(0, message.IndexOf("\0"));
                Console.WriteLine(message);
            }

        }

    }
}

