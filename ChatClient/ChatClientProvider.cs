using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient
{
    // Provide a global client library. For use with console applications only for now.
    public class ChatClientProvider : IDisposable
    {
        private readonly string MyId;
        private readonly string OtherClientId;
        private readonly string ServerName = "localhost";
        private readonly int PortNumber = 7890;

        public ChatClientProvider(string clientId, string forClientId)
        {
            MyId = clientId;
            OtherClientId = forClientId;
        }

        TcpClient _client;
        NetworkStream _stream;

        public void Execute()
        {
            // Create a client to connect using TCP and a stream to share messages.
            _client = new TcpClient();
            string response = String.Empty;

            try
            {
                Console.WriteLine(String.Format("This is Client-ID: {0}", MyId));
                Console.WriteLine(String.Format("Connecting to server [{0}] on port [{1}].", ServerName, PortNumber));

                _client.Connect(ServerName, PortNumber);
                _stream = _client.GetStream();
                response = SendRecieve(MyId);

                if (response == "|ACK|")
                {
                    Console.WriteLine("Connected");
                }
                else
                {
                    Console.WriteLine("Unable to connect..");
                    Console.ReadLine();
                    return;
                }

                Console.WriteLine("Start chatting..\n(PS: To exit, simply close the window.)");

                while (true)
                {
                    Console.WriteLine("Type your message: ");
                    string chatMessage = Console.ReadLine();

                    // Add forClientId
                    chatMessage = String.Format("{0}|~~|{1}", OtherClientId, chatMessage);

                    response = SendRecieve(chatMessage);
                    Console.WriteLine("Response(s): " + response);
                }
            }
            catch (SocketException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (_stream != null)
                {
                    _stream.Close();
                }

                if (_client.Connected)
                {
                    _client.Close();
                }
            }

        }

        string SendRecieve(string message)
        {
            string retval;

            // This is the way to read from the server stream
            _stream = _client.GetStream();
            byte[] sendBuffer = Encoding.UTF8.GetBytes(message);
            _stream.Write(sendBuffer, 0, sendBuffer.Length);

            Console.WriteLine("Receiving..");
            byte[] recieveBuffer = new byte[_client.ReceiveBufferSize];
            int recieved = _stream.Read(recieveBuffer, 0, _client.ReceiveBufferSize);
            retval = Encoding.UTF8.GetString(recieveBuffer).Trim('\0');

            return retval;
        }
        
        public void Dispose()
        {
            SendRecieve("|FIN|");
        }
    }
}
