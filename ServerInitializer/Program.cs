using ChatServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerInitializer
{
    class Program
    {
        private static TestServer _server;
        private const int Port = 7890;

        static void Main(string[] args)
        {
            InitializeTestServer();
            Console.WriteLine("Server is listening on " + Port + ". Hit enter to exit..");
            Console.ReadLine();
        }

        private static void InitializeTestServer()
        {
            _server = new TestServer(Port);
            _server.Start();
        }

        ~Program()
        {
            if (_server != null)
            {
                _server.Stop();
            }
        }
    }
}
