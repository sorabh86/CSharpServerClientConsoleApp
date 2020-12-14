namespace Bindings
{
    // Get/Set from Server to Client; 
    // Client has to listen to Server Packets
    public enum ServerPackets
    {
        SConnectionOk = 1,
    }

    // Get/Set from Client to Server;
    // Server has to listen to Client Packets
    public enum ClientPackets
    {
        CThankYou = 1,
    }
}