using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;


namespace TcpFileClient{
    public class MyTcpClient
    {
        private static int port = 9000;
        private static TcpClient client = null;
        private static byte[] outputBuffer = new byte[1024];
        private static byte[] inputBuffer = new byte[4096];
        private static string receivedString = null;
 
        public static void Main(string[] args) 
        {
            string serverIP = args[0];
            string filepath = args[1];

            Console.WriteLine("Attempting to connect to server at IP {0}", serverIP);
            Connect(serverIP);

            Console.WriteLine("Requesting file {0} from server", filepath);
            GetFile(filepath);
        }

        private static TcpClient Connect(string server) 
        {
            try
            {
                client = new TcpClient(server, port);   
            }
            catch (System.Exception e)
            {
                Console.WriteLine("Failed to connect: {0}", e.ToString());
            }

            return client;
        }

        private static void GetFile(string path)
        {
            // Encode string to bytes
            outputBuffer = System.Text.Encoding.ASCII.GetBytes(path);    

            // Set up the stream (to read and write from)
            NetworkStream stream = client.GetStream();

            // Send the request to the connected TcpServer. 
            stream.Write(outputBuffer, 0, outputBuffer.Length);

            Console.WriteLine("Requested file.");         
        
            // Read filesize from stream
            int bytesRead = stream.Read(inputBuffer, 0, inputBuffer.Length);

            // To string
            receivedString = System.Text.Encoding.ASCII.GetString(inputBuffer,0,bytesRead);
            Console.WriteLine("Received: {0}", receivedString);

            if (receivedString == "0") { //Means that file doesn't exist or is empty
                Console.WriteLine("File requested doesn't exist or is empty");
            } else {
                Console.WriteLine("File {0} exists and is {1} bytes large",path,receivedString);

                SendAck(stream);
                
                // Create new byte array that is the size of the incoming file
                byte[] truncatedBuffer = new byte[Convert.ToInt32(receivedString)];

                bytesRead = stream.Read(truncatedBuffer,0,truncatedBuffer.Length);
                //receivedString = Encoding.ASCII.GetString(inputBuffer,0,bytesRead);

                File.WriteAllBytes("received_"+path,truncatedBuffer);
                //Console.WriteLine(receivedString);
                Console.WriteLine("Wrote to file");
            }
        }

        private static void SendAck(NetworkStream stream)
        {
            outputBuffer = Encoding.ASCII.GetBytes("1");
            stream.Write(outputBuffer,0,outputBuffer.Length); //ack
        }
    }
}