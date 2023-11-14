// See https://aka.ms/new-console-template for more information
using Server;


//TestServer.Main();

//TCPServer server = new(5050);
//server.Start();

AsynchronousTestServer asynchronousTestServer = new(8001);
asynchronousTestServer.StartListenAsync();
