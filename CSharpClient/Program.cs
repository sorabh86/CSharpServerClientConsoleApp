using System;

namespace CSharpClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Client";
            ClientDataHandle.InitializeNetworkPackages();
            ClientTCP.ConnectToServer();
            Console.ReadLine();
        }
    }
}
