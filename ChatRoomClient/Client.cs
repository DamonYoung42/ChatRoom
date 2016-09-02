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
                clientStream.Connect("localhost", 10000);
                Console.WriteLine("Connected to Chat Server ...");
                Console.WriteLine("What is your name?");
                string userName = Console.ReadLine();
                byte[] outStream = System.Text.Encoding.ASCII.GetBytes(userName);
                serverStream = clientStream.GetStream();
                serverStream.Write(outStream, 0, outStream.Length);
                serverStream.Flush();

                while (clientStream.Connected)
                {
                    Thread receiveMessage = new Thread(GetMessage);
                    receiveMessage.Start();
                    serverStream = clientStream.GetStream();
                    string message = Console.ReadLine();

                    byte[] outMessage = Encoding.Default.GetBytes(message);
                    serverStream.Write(outMessage, 0, outMessage.Length);
                    serverStream.Flush();

                    if (message.ToLower() == "exit")
                    {
                        Environment.Exit(0);
                    }


                }
            }
            catch (Exception error)
            {
                Console.WriteLine(error);

            }
            finally
            {
                if (clientStream != null)
                {
                    clientStream.Close();
                }
            }
        }


        private void GetMessage()
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

