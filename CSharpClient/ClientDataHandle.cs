using System;
using System.Collections.Generic;
using Bindings;

namespace CSharpClient
{
    public class ClientDataHandle
    {
        private delegate void Packet_(byte[] data);
        private static Dictionary<int, Packet_> Packets;

        public static void InitializeNetworkPackages()
        {
            Console.WriteLine("Initialize Network Packages");
            Packets = new Dictionary<int, Packet_>
            {
                {(int)ServerPackets.SConnectionOk, HandleConnectionOk}
            };
        }

        public static void HandleNetworkInformation(byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteBytes(data);
            int packetnum = buffer.ReadInteger();
            buffer.Dispose();

            // Gets the value associated with the specified key.
            if(Packets.TryGetValue(packetnum, out Packet_ Packet))
            {
                Packet.Invoke(data);
            }
        }

        private static void HandleConnectionOk(byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteBytes(data);
            buffer.ReadInteger();
            string msg = buffer.ReadString();
            buffer.Dispose();

            // Add your code you want to execute here:
            Console.WriteLine(msg);

            ClientTCP.ThankYouServer();
        }
    }
}