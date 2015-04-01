using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer
{
    internal class ChatHandler
    {
        // TODO: implement periodic check on connected clients to remove
        //..the ones not responding/connected anymore (for some reason).
        private Dictionary<Guid, Socket> _connectedClients;
        private object _locker;

        public ChatHandler()
        {
            _connectedClients = new Dictionary<Guid, Socket>();
            _locker = new object();
        }

        public bool RouteChat(Guid fromClientId, Guid forClientId, string message)
        {
            bool retval = false;
            Socket forClientSocket = null;

            try
            {
                // Synchronize access to dict and grab 'forClientId' socket.
                // May be synchronization not required for this is just a read
                if (_connectedClients.ContainsKey(forClientId))
                {
                    forClientSocket = _connectedClients[forClientId];

                    // Verify if the client is connected and send message
                    SendMessage(forClientSocket, message);

                    // Return true if message sent successfully
                    retval = true;
                }
            }
            catch (Exception ex)
            {
                // Just throw for now..
                throw ex;
            }

            return retval;
        }

        public bool AddClient(string negotiationToken, Socket clientSocket, ref Guid clientId)
        {
            bool retval = false;

            lock (_locker)
            {
                // Extract and validate negotiation token (client id)
                if (Guid.TryParse(negotiationToken, out clientId) && !_connectedClients.ContainsKey(clientId))
                {
                    _connectedClients.Add(clientId, clientSocket);
                    retval = true;
                }
            }
            return retval;
        }

        public void RemoveClient(Guid clientId)
        {
            lock (_locker)
            {
                if (_connectedClients.ContainsKey(clientId))
                {
                    _connectedClients[clientId].Close();
                    _connectedClients.Remove(clientId);
                }
            }
        }

        public void EndAllConnections()
        {
            foreach (Socket socket in _connectedClients.Values)
            {
                socket.Close();
            }
        }

        internal void SendMessage(Socket clientSocket, string message)
        {
            if (clientSocket != null)
            {
                UTF8Encoding encoder = new UTF8Encoding();
                Byte[] buffer = encoder.GetBytes(message);

                clientSocket.Send(buffer, buffer.Length, 0);
            }
        }


    }
}
