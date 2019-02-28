using System;
using System.Net;
using System.Net.Sockets;
using System.Text;


namespace TcpFileClient{
    public class MyTcpClient{
        private static int port = 9000;
        private static TcpClient client = null;
        private static byte[] outputBuffer = new byte[1024];
        private static string outputString = null;
 
        public static void Main(string[] args) {
            string serverIP = args[0];
            string filepath = args[1];

            Console.WriteLine("Attempting to get file '{0}' from IP {1}", filepath, serverIP);
            Connect(serverIP, filepath);
        }

        static void Connect(string server, string message) 
            {
                try
                {
                    client = new TcpClient(server, port);   
                }
                catch (System.Exception e)
                {
                    Console.WriteLine("Failed to connect: {0}", e.ToString());
                }
                
                // Encode string to bytes
                outputBuffer = System.Text.Encoding.ASCII.GetBytes(message);    

                // Set up the stream (to read and write from)
                NetworkStream stream = client.GetStream();

                // Send the message to the connected TcpServer. 
                stream.Write(outputBuffer, 0, outputBuffer.Length);

                Console.WriteLine("Requested file.");         

                /* // Receive the TcpServer.response.
                
                // Buffer to store the response bytes.
                data = new Byte[256];

                // String to store the response ASCII representation.
                String responseData = String.Empty;

                // Read the first batch of the TcpServer response bytes.
                Int32 bytes = stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                Console.WriteLine("Received: {0}", responseData);         

                // Close everything.
                stream.Close();         
                client.Close();          */
            }
    }
}