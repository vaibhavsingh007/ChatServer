using ChatClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient1
{
    class Program
    {
        static ChatClientProvider client1;

        static void Main(string[] args)
        {
            string id = "B55DDF6A-072E-40CC-A20F-A7BA8C0972F5";
            string otherClientId = "E5DBBEB9-BAD2-4B6F-957D-2DDC4305AE09";

            using (client1 = new ChatClientProvider(id, otherClientId))
            {
                client1.Execute();
            }
        }

        ~Program()
        {
            if (client1 != null)
            {
                client1.Dispose();
            }
        }
    }
}
