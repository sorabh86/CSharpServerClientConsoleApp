using System;

namespace CSharpServer.App
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Server";
            ServerDataHandle.InitializeNetworkPackages();
            ServerTCP.SetupServer();
            Console.ReadLine();
        }
    }
}
