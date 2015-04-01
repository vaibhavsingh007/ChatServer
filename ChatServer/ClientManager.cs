using ChatServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace ChatServer
{
    internal class ClientManager
    {
        private Socket _clientSocket;
        private ChatHandler _chatHandler;
        private Guid _myId;

        public ClientManager(Guid myId, Socket clientSocket, ChatHandler chatHandler)
        {
            _clientSocket = clientSocket;
            _chatHandler = chatHandler;
            _myId = myId;

            // Start constant listener
            HandleClientMessages();
        }

        private void HandleClientMessages()
        {
            int recvStatus = -1;
            string message = String.Empty;
            string forClientIdString = String.Empty;
            Guid forClientId = Guid.Empty;
            Byte[] buffer = null;

            while (true)
            {
                // Wait for messages from the client
                buffer = new byte[_clientSocket.ReceiveBufferSize];
                recvStatus = _clientSocket.Receive(buffer);

                // Decode the UTF8 string
                message = Encoding.UTF8.GetString(buffer).Trim('\0');

                // Verify if close-connection request (|FIN|)
                if (message.Equals(Constants.ConnectionSymbols.EndConnection))
                {
                    EndConnection();
                }
                else
                {
                    if (message.Contains(Constants.ChatSymbols.ForClientIdDelimiter))
                    {
                        string[] splits = message.Split(new string[] { Constants.ChatSymbols.ForClientIdDelimiter },
                                                StringSplitOptions.None);
                        forClientIdString = splits[0];
                        message = splits[1];

                        if (Guid.TryParse(forClientIdString, out forClientId))
                        {
                            // Pass message to Chat Router
                            _chatHandler.RouteChat(_myId, forClientId, message);
                        }
                    }
                }
            }

        }

        private void EndConnection()
        {
            if (_clientSocket != null)
            {
                _chatHandler.RemoveClient(_myId);
            }
        }
    }
}
