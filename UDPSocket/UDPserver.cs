using System;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace UDP
{
    public class UDPServer
    {
        private static int PORT = 9000;
        private static string receivedString;
        public static void Main()
        {
            byte[] data = new byte[1024];

            //Setup up end point for this server - any IP, port defined in class
            IPEndPoint _endPoint = new IPEndPoint(IPAddress.Any, PORT);
            UdpClient udpSocket = new UdpClient(_endPoint);
            Console.WriteLine("Server created: {0}", _endPoint.ToString());

            // Start server
            Console.WriteLine("Waiting for client connection...");

            // Listen for connections on ANY IP address on ANY available port (0)
            IPEndPoint client = new IPEndPoint(IPAddress.Any, 0);

            // Blocking (synchronous) receive
            data = udpSocket.Receive(ref client);

            Console.WriteLine("Message received from {0}:", client.ToString());
            Console.WriteLine(Encoding.ASCII.GetString(data, 0, data.Length));

            // Send connection response to client
            string connectMsg = "Connected to server";
            data = Encoding.ASCII.GetBytes(connectMsg);
            udpSocket.Send(data, data.Length, client);

            while (true)
            {
                // Handle requests
                data = udpSocket.Receive(ref client);
                Console.WriteLine("Received message {0} from {1}", Encoding.ASCII.GetString(data), client.ToString());

                receivedString = Encoding.ASCII.GetString(data, 0, data.Length).ToLower();
                if (receivedString == "l")
                {
                    data = Encoding.ASCII.GetBytes("200");
                    udpSocket.Send(data, data.Length, client);
                    data = File.ReadAllBytes("/proc/loadavg");
                    udpSocket.Send(data, data.Length, client);
                }
                else if (receivedString == "u")
                {
                    data = Encoding.ASCII.GetBytes("200");
                    udpSocket.Send(data, data.Length, client);
                    data = File.ReadAllBytes("/proc/uptime");
                }
                else
                {
                    // If command is neither l, L, u or U, send "Bad Request"
                    data = Encoding.ASCII.GetBytes("400");
                    udpSocket.Send(data, data.Length, client);
                }
            }
        }
    }
}