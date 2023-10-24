// See https://aka.ms/new-console-template for more information
using Server;

Console.WriteLine("Hello, World!");

//TestServer.Main();

AsynchronousTestServer asynchronousTestServer = new(8001);
asynchronousTestServer.StartListenAsync();