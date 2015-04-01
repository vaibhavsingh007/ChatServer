using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChatServer
{
    public class TestServer
    {
        private TcpListener _listener;
        private Thread _listenerThread;
        private int _port;
        private ChatHandler _chatHandler;

        public TestServer(int port)
        {
            _port = port;
            _chatHandler = new ChatHandler();
        }

        // Creating the Start method to handle a start of the server
        public void Start()
        {
            _listenerThread = new Thread(Listener);
            _listenerThread.IsBackground = true;
            _listenerThread.Name = "Listener";
            _listenerThread.Start();
        }

        // Now creating the Listener method that will be fired by the thread
        public void Listener()
        {
            try
            {
                int receiveStatus = 0;
                byte[] buffer = null;
                string connectionResponse = String.Empty;
                Guid clientId = Guid.Empty;
                IPAddress ip = IPAddress.Parse("127.0.0.1");
                _listener = new TcpListener(ip, _port);
                _listener.Start();

                while (true)    // For the AcceptSocket to wait for the client to connect
                {
                    Socket clientSocket = _listener.AcceptSocket();

                    buffer = new byte[clientSocket.ReceiveBufferSize];
                    receiveStatus = clientSocket.Receive(buffer);
                    string clientIdString = Encoding.UTF8.GetString(buffer).Trim('\0');

                    // This must be a valid client id (GUID) to establish connection
                    if (_chatHandler.AddClient(clientIdString, clientSocket, ref clientId))
                    {
                        connectionResponse = Constants.ConnectionSymbols.SuccessfulConnection;
                        Task.Factory.StartNew(() => (new ClientManager(clientId, clientSocket, _chatHandler)));
                    }
                    else
                    {
                        connectionResponse = Constants.ConnectionSymbols.UnsuccessfulConnection;
                    }

                    // Prepare to send the message in unicode as follows:
                    _chatHandler.SendMessage(clientSocket, connectionResponse);
                }
            }
            catch (SocketException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        // Write the other control methods like,
        public void Stop()
        {
            _chatHandler.EndAllConnections();
            _listener.Stop();
        }
        public void Suspend()
        {
            _listener.Stop();
        }
        public void Resume()
        {
            Start();
        }
    }
}
