using System;
using System.Net;
using System.Net.Sockets;
using Bindings;

namespace CSharpServer.App
{
    public class ServerTCP
    {
        private static Socket _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private static byte[] _buffer = new byte[1024];

        public static Client[] _clients = new Client[Constants.MAX_PLAYERS];

        public static void SetupServer()
        {
            for(int i = 0; i < Constants.MAX_PLAYERS; i++) {
                _clients[i] = new Client();
            }

            // who can connect to server and which port is used
            _serverSocket.Bind(
                new IPEndPoint(IPAddress.Any, 5555)
            );
            // 10 peoples can be connected to the server at the same time
            _serverSocket.Listen(10);

            _serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);

            Console.WriteLine("Server is running...");
        }

        /** Runs when client send connection request to server
         * once connected keep continue accepting more requests
         */
        public static void AcceptCallback(IAsyncResult ar)
        {
            Socket socket = _serverSocket.EndAccept(ar);
            _serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);

            for (int i = 0; i < Constants.MAX_PLAYERS; i++)
            {
                if(_clients[i].socket == null) {
                    _clients[i].socket = socket;
                    _clients[i].index = i;
                    _clients[i].ip = socket.RemoteEndPoint.ToString();
                    // Start socket to continue listen socket for new packets.
                    _clients[i].StartClient();

                    Console.WriteLine("Connection from '{0}' received.", _clients[i].ip);

                    // Server Server to Client message(to byte array)
                    SendConnectionOk(i);
                    return;
                }
            }
        }

        public static void SendDataTo(int index, byte[] data)
        {
            byte[] sizeinfo = new byte[4];
            sizeinfo[0] = (byte)data.Length;
            sizeinfo[1] = (byte)(data.Length >> 8);
            sizeinfo[2] = (byte)(data.Length >> 16);
            sizeinfo[3] = (byte)(data.Length >> 24);

            _clients[index].socket.Send(sizeinfo);            
            _clients[index].socket.Send(data);
        }

        public static void SendConnectionOk(int index)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteInteger((int)ServerPackets.SConnectionOk);
            buffer.WriteString("You are successfully connected to the server.");
            SendDataTo(index, buffer.ToArray());
            buffer.Dispose();
        } 
        
    }

    public class Client
    {
        public int index;
        public string ip;
        public Socket socket;
        public bool closing = false;
        private byte[] _buffer = new byte[1024];

        public void StartClient()
        {
            socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), socket);
            closing = false;
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            Socket socket = (Socket)ar.AsyncState;

            try
            {
                int received = socket.EndReceive(ar);
                if(received <= 0)
                {
                    CloseClient(index);
                } else {
                    byte[] databuffer = new byte[received];
                    Array.Copy(_buffer, databuffer, received);

                    // TODO: HandleNetworkInformation
                    ServerDataHandle.HandleNetworkInformation(index, databuffer);

                    socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), socket);
                }
            } catch {
                CloseClient(index);
            }
        }

        private void CloseClient(int index)
        {
            closing = true;
            Console.WriteLine("Connection from {0} has been terminated.", ip);
            // PlayerLeftGame;
            socket.Close();
        }
    }
}