using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoomServer
{
    class Participant : IParticipant
    {
        string name;
        //int port;

        public Participant(string name)
        {
            this.name = name;
        }

        public void Update()
        {
            Console.WriteLine("Another person joined the Chat Room.");
        }



    }
}
