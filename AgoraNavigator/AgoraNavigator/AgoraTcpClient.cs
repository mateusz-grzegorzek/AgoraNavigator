using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace AgoraNavigator
{
    class AgoraTcpClient
    {
        static TcpClient client;
        static NetworkStream stream;
        private static byte[] data;
        //public static bool responseReceived;

        public static void TcpClientThread()
        {
            Console.WriteLine("TcpClient:TcpClientThread started!");
            client = new TcpClient("YOUR COMPUTER IP FINDING BY IPCONFIG ", 4444); //Trys to Connect
            //ClientReceive(); //Starts Receiving When Connected
        }

        public static String ClientReceive()
        {
            String result = "dummy";
            //byte[] data;
            try
            {
                stream = client.GetStream(); //Gets The Stream of The Connection
                //new Thread(() => // Thread (like Timer)
                //{
                    int i;
                    byte[] datalength = new byte[4];
                    while ((i = stream.Read(datalength, 0, 4)) != 0)//Keeps Trying to Receive the Size of the Message or Data
                    {
                        // how to make a byte E.X byte[] examlpe = new byte[the size of the byte here] , i used BitConverter.ToInt32(datalength,0) cuz i received the length of the data in byte called datalength :D
                        data = new byte[BitConverter.ToInt32(datalength, 0)]; // Creates a Byte for the data to be Received On
                        stream.Read(data, 0, data.Length); //Receives The Real Data not the Size
                        //responseReceived = true;
                    }
                //}).Start(); // Start the Thread
            }
            catch (Exception ex)
            {
                Console.WriteLine("AgoraTcpClient:clientReceive:Exception=" + ex.ToString());
            }
            if (data != null)
            {
                result = Encoding.UTF8.GetString(data);
            }
            return result;
        }

        public static void ClientSend(string msg)
        {
            //responseReceived = false;
            try
            {
                stream = client.GetStream(); //Gets The Stream of The Connection
                byte[] data; // creates a new byte without mentioning the size of it cuz its a byte used for sending
                data = Encoding.Default.GetBytes(msg); // put the msg in the byte ( it automaticly uses the size of the msg )
                int length = data.Length; // Gets the length of the byte data
                byte[] datalength = new byte[4]; // Creates a new byte with length of 4
                datalength = BitConverter.GetBytes(length); //put the length in a byte to send it
                stream.Write(datalength, 0, 4); // sends the data's length
                stream.Write(data, 0, data.Length); //Sends the real data
            }
            catch (Exception ex)
            {
                Console.WriteLine("AgoraTcpClient:clientReceive:Exception=" + ex.ToString());
            }
        }
    }
}
