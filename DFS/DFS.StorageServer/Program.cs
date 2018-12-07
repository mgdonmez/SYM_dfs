using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DFS.StorageServer
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                IPAddress ipaddress = IPAddress.Parse("127.0.0.1");
                TcpListener mylist = new TcpListener(ipaddress, 8000);
                while (Console.ReadLine() != "exit")
                {                    
                    mylist.Start();
                    Console.WriteLine("Server is Running on Port: 8000");
                    Console.WriteLine("Local endpoint:" + mylist.LocalEndpoint);
                    Console.WriteLine("Waiting for Connections...");
                    Socket socket = mylist.AcceptSocket();
                    Console.WriteLine("Connection Accepted From:" + socket.RemoteEndPoint);
                    byte[] receivedMessage = new byte[100];
                    int k = socket.Receive(receivedMessage);
                    Console.WriteLine("Recieved..");

                    //handle received message!!!

                    ASCIIEncoding asencd = new ASCIIEncoding();

                    //send in içine feedback (data, op sonucu)
                    socket.Send(asencd.GetBytes("Automatic Message:" + "String Received byte server !"));
                    Console.WriteLine("\nAutomatic Message is Sent");
                    socket.Close();
                }
                mylist.Stop();
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error.." + ex.StackTrace);
                Console.ReadLine();
            }
        }
    }
}
