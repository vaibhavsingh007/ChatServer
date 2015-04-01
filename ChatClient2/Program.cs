using ChatClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient2
{
    class Program
    {
        static void Main(string[] args)
        {
            string id = "E5DBBEB9-BAD2-4B6F-957D-2DDC4305AE09";
            string otherClientId = "B55DDF6A-072E-40CC-A20F-A7BA8C0972F5";

            using (ChatClientProvider client2 = new ChatClientProvider(id, otherClientId))
            {
                client2.Execute();
            }
        }
    }
}
