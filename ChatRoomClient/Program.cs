using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

using System.IO;

namespace ChatRoomClient
{
    class Program
    {
        static void Main(string[] args)
        {

            Client client = new ChatRoomClient.Client();
            client.RunClient();

        }
    }

}
