using System;
using System.Collections.Generic;
using Bindings;

namespace CSharpServer
{
    public class ServerDataHandle
    {
        private delegate void Packet_(int index, byte[] data);
        private static Dictionary<int, Packet_> Packets;

        public static void InitializeNetworkPackages()
        {
            Console.WriteLine("Initialize Network Packages");
            Packets = new Dictionary<int, Packet_>
            {
                {(int)ClientPackets.CThankYou, HandleThankYou}
            };
        }

        public static void HandleNetworkInformation(int index, byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteBytes(data);
            int packetnum = buffer.ReadInteger();
            buffer.Dispose();

            if(Packets.TryGetValue(packetnum, out Packet_ Packet))
            {
                Packet.Invoke(index, data);
            }
        }

        private static void HandleThankYou(int index, byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteBytes(data);
            buffer.ReadInteger();
            string msg = buffer.ReadString();
            buffer.Dispose();

            //add your code you want to execute here:
            Console.WriteLine(msg);
        }
    }
}