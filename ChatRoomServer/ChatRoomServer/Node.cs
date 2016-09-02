using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace ChatRoomServer
{
    public class Node
    {
            public string name;
            public TcpClient tcpClient;
            public Node left;
            public Node right;

            // Constructor  to create a single node 
            public Node(string name, TcpClient client)
            {
                this.name = name;
                tcpClient = client;
                this.left = null;
                this.right = null;
            } 
    }
}
