using System;
using System.Net.Sockets;

namespace Web_API.Controllers.House
{
    public class House
    {
        public TcpClient client;
        public NetworkStream stream;

        public bool Connect(out ApiResult result)
        {
            result = null;

            client = new TcpClient();
            IAsyncResult connection = client.BeginConnect(Config.House.ADDRESS, Config.House.PORT, null, null);
            //Set timeout to 2 seconds.
            bool success = connection.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(2));

            if (success == false || client.Connected == false)
            {
                result = new ApiResult("Could not connect to DaHaus.", false);
                return false;
            }

            // Get a client stream for reading and writing.
            stream = client.GetStream();
            return true;
        }

        public string GetResponse(String message)
        {
            message += Environment.NewLine;

            this.Send(message);

            return this.Read().Trim();
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
