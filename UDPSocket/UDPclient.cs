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
        public static int Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("First argument must be the IP of the server you wish to connect to. Terminating...");
                return 1;
            }
            IPAddress serverIP = IPAddress.Parse(args[0]);

            UdpClient udpClient = new UdpClient(PORT);
            try
            {
                // Create IP End point
                IPEndPoint server = new IPEndPoint(serverIP, PORT);

                Console.WriteLine("Connecting to server: {0}", server.ToString());
                udpClient.Connect(server);

                // Send request to server
                byte[] sendBytes = Encoding.ASCII.GetBytes("connect");
                udpClient.Send(sendBytes, sendBytes.Length);

                // Blocks until a response returns on this socket from the connected server
                Console.WriteLine("Waiting for response...");
                byte[] receiveBytes = udpClient.Receive(ref server);
                string returnData = Encoding.ASCII.GetString(receiveBytes);

                Console.WriteLine("Received response from {0}: {1}", server.ToString(), returnData);

                Console.WriteLine("Write a request for the server: ");
                while (true)
                {
                    sendBytes = Encoding.ASCII.GetBytes(Console.ReadLine());
                    udpClient.Send(sendBytes, sendBytes.Length);
                }

                udpClient.Close();
                return 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return 1;
            }
        }
    }
}