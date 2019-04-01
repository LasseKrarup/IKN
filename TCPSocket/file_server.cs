using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;
using System.IO;


namespace TcpFileServer
{
    public class MyTcpServer
    {
        private static int port = 9000;
        private static TcpListener server = null;
        private static byte[] recBuffer = new byte[1000];
        private static string receivedString = null;
        private static byte[] outputFile = new byte[4096];
        private static byte[] outBuf = new byte[1000];
        private static string outMsg = null;

        public static void Main(string[] args)
        {
            SetupServer();
            while (true)
            {
                TcpClient clientHandler = listenForClient();
            }
        }

        private static void SetupServer()
        {
            server = new TcpListener(IPAddress.Any, port);

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

        private static TcpClient listenForClient()
        {
            Console.WriteLine("Listening...");

            TcpClient clientHandler = server.AcceptTcpClient(); //returns the connected TCP client object

            NetworkStream dataStream = clientHandler.GetStream();
            int bytesRead = dataStream.Read(recBuffer, 0, 1000); //buffer, place to start in buffer, max number of bytes to read
            receivedString = Encoding.ASCII.GetString(recBuffer, 0, bytesRead);

            Console.WriteLine("Received from client: '{0}' with length {1}", receivedString, receivedString.Length.ToString());

            if (File.Exists(receivedString))
            {
                outputFile = File.ReadAllBytes(receivedString);
                Console.WriteLine("File is {0} bytes long", outputFile.Length.ToString());
                // int numberOfPackages = (int)Math.Ceiling((double)outputFile.Length/1000);
                // for (int i = 0; i < numberOfPackages; i++) {
                //     dataStream.Write(outputFile,i*1000,1000);
                // }

                // Send file size if it exists
                int len = outputFile.Length;
                outMsg = len.ToString();
                byte[] outBuf = Encoding.ASCII.GetBytes(outMsg);
                dataStream.Write(outBuf, 0, outBuf.Length);

                bytesRead = dataStream.Read(recBuffer, 0, recBuffer.Length); //Get acknowledgement
                receivedString = Encoding.ASCII.GetString(recBuffer, 0, bytesRead);
                if (receivedString == "1")
                {// if ack is true
                    Console.WriteLine("Received acknowledgement from client. Sending file");
                    dataStream.Write(outputFile, 0, outputFile.Length);
                }
            }
            else
            {
                outBuf = Encoding.ASCII.GetBytes("0");
                dataStream.Write(outBuf, 0, outBuf.Length); //Write a 0 signaling file doesn't exist or is empty
            }

            return clientHandler;
        }
    }
}