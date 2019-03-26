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

        public static void Main()
        {
            byte[] data = new byte[1024];

            //Setup up end point for this server - any IP, port defined in class
            IPEndPoint _endPoint = new IPEndPoint(IPAddress.Any, PORT);
            UdpClient udpSocket = new UdpClient(_endPoint);
            Console.WriteLine("Server created: {0}", _endPoint.ToString());

            // Start server
            Console.WriteLine("Waiting for client connection...");

            // Listen for ANY IP address on ANY available port (0)
            IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);

            // Blocking (synchronous) receive
            data = udpSocket.Receive(ref sender);

            Console.WriteLine("Message received from {0}:", sender.ToString());
            Console.WriteLine(Encoding.ASCII.GetString(data, 0, data.Length));

            // Send connection response to client
            string connectMsg = "Connected to server";
            data = Encoding.ASCII.GetBytes(connectMsg);
            udpSocket.Send(data, data.Length, sender);

            while (true)
            {
                // Handle requests
                data = udpSocket.Receive(ref sender);
                Console.WriteLine("Received message from {0}", sender.ToString());

                // Echo back request
                Console.WriteLine(Encoding.ASCII.GetString(data, 0, data.Length));
                udpSocket.Send(data, data.Length, sender);
                Console.WriteLine("Echoed {1} back to {0}", data, sender.ToString());
            }
        }
    }
}