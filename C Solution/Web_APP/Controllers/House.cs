using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Sockets;

namespace Web_API.Controllers
{
    public class House
    {
        public static string server_address = "127.0.0.1";
        public int server_port = 11000;
        public TcpClient client;
        public NetworkStream stream;

        public House() {
            // Create a TcpClient.
            client = new TcpClient(server_address, server_port);

            // Get a client stream for reading and writing.
            stream = client.GetStream();
        }

        public string GetState(String message)
        {
            message += Environment.NewLine;

            Send(message);

            return Read().Trim();
        }

        public string Read()
        {
            // Buffer to store the response bytes.
            byte[] dataBuffer = new byte[client.ReceiveBufferSize];

            // Read the first batch of the TcpServer response bytes.
            int BytesRead = stream.Read(dataBuffer, 0, dataBuffer.Length);

            // Recived bytes to string
            return System.Text.Encoding.ASCII.GetString(dataBuffer, 0, BytesRead);
        }


        public void Send(string message)
        {
            // Translate the passed message into ASCII and store it as a Byte array.
            byte[] dataBuffer = System.Text.Encoding.ASCII.GetBytes(message);

            // Send the message to the connected TcpServer. 
            stream.Write(dataBuffer, 0, dataBuffer.Length);
        }

        public void Close()
        {
            // Close everything.
            byte[] dataBuffer = System.Text.Encoding.ASCII.GetBytes("exit" + Environment.NewLine);
            stream.Write(dataBuffer, 0, dataBuffer.Length);

            // Buffer to store the response bytes.
            dataBuffer = new byte[client.ReceiveBufferSize];
            stream.Read(dataBuffer, 0, dataBuffer.Length);

            client.Close();
            stream.Close();
        }
    }
}