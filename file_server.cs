using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;
using System.IO;


namespace TcpFileServer{
    public class MyTcpServer{
        private static int port = 9000;
        private static TcpListener server = null;
        private static byte[] recBuffer = new byte[1024];
        private static string receivedString = null;
        private static byte[] outputFile = new byte[4096];
 
        public static void Main(string[] args) {
            SetupServer();
            while(true) {
                TcpClient clientHandler = listenForClient();
            }
        }

        private static void SetupServer(){
            IPAddress localIp = IPAddress.Parse("127.0.0.1");
            server = new TcpListener(localIp,port);

            if (File.Exists("test.txt")){
                Console.WriteLine("Yes");
            } else { 
                Console.WriteLine("No");
            }

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

            Console.WriteLine(receivedString);
            if(receivedString == "test.txt") {
                Console.WriteLine("Yes");
            } else {
                Console.WriteLine("No");
            }

/*             if (File.Exists(receivedString)) {
                outputFile = File.ReadAllBytes(receivedString);
                int numberOfPackages = (int)Math.Ceiling((double)outputFile.Length/1000);
                for (int i = 0; i < numberOfPackages; i++) {
                    dataStream.Write(outputFile,i*1000,1000);
                }
            } else {
                string msg = "err: file doesnt exist";
                byte[] msgToSend = Encoding.ASCII.GetBytes(msg);
                dataStream.Write(msgToSend, 0, msgToSend.Length);
            } */

            return clientHandler;
        }
    }
}