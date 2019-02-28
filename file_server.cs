using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;


namespace TcpFileServer{
    public class MyTcpServer{
        private static int port = 9000;
        private static TcpListener server = null;
        private static byte[] recBuffer = new byte[1024];
        private static string receivedString = null;
        private static string returnString = null;
 
        public static void Main(string[] args) {
            SetupServer();
            while(true) {
                TcpClient clientHandler = listenForClient();
            }
        }

        private static void SetupServer(){
            IPAddress localIp = IPAddress.Parse("127.0.0.1");
            server = new TcpListener(localIp,port);
            try
            {
                server.Start();
                Console.WriteLine("Server started...");
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private static TcpClient listenForClient(){
            Console.WriteLine("Listening...");

            TcpClient clientHandler = server.AcceptTcpClient(); //returns the connected TCP client object

            NetworkStream dataStream = clientHandler.GetStream();
            dataStream.Read(recBuffer, 0, 1000); //buffer, place to start in buffer, number of bytes read
            receivedString = Encoding.ASCII.GetString(recBuffer);
            Console.WriteLine(receivedString);

            return clientHandler;
        }
    }
}