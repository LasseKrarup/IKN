using System;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace UDP
{
    public class myUdpClient
    {
        private static int PORT = 9000;
        public static void Main(string[] args)
        {
            Console.WriteLine("Starting main with args: {0} and {1}", args[0], args[1]);

            Console.WriteLine("Creating IPAddress");
            IPAddress serverIP = IPAddress.Parse(args[0]);
            string cmd = args[1];


            Console.WriteLine("Creating UdpClient");
            UdpClient udpClient = new UdpClient(PORT);
            try
            {

                Console.WriteLine("Creating IP end point");
                IPEndPoint server = new IPEndPoint(serverIP, PORT);


                Console.WriteLine("Connecting to server: {0}", server.ToString());
                udpClient.Connect(server);

                // Send request to server
                byte[] sendBytes = Encoding.ASCII.GetBytes(cmd);
                udpClient.Send(sendBytes, sendBytes.Length);
                Console.WriteLine("Request sent to server: {0}. Byte length: {1}", cmd, sendBytes.Length);

                // Blocks until a response returns on this socket from the connected server
                Console.WriteLine("Waiting for response...");
                byte[] receiveBytes = udpClient.Receive(ref server);
                string returnData = Encoding.ASCII.GetString(receiveBytes);

                Console.WriteLine("Received response from {0}: {1}", server.ToString(), returnData);

                udpClient.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}