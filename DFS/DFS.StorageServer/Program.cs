using System;
using System.Collections.Generic;
using System.IO;
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
                    byte[] receivedMessage = new byte[1000];
                    int k = socket.Receive(receivedMessage);
                    Console.WriteLine("Recieved..");
                    string message = Encoding.ASCII.GetString(receivedMessage);
                    ASCIIEncoding asencd = new ASCIIEncoding();
                    string[] arguements = message.Split(' ');
                    string path = arguements[1];
                    Console.WriteLine(arguements[0]);
                    Console.WriteLine(arguements[1]);

                    switch (arguements[0])
                    {
                        case "LST":
                            StringBuilder builder = ReadFileAttributes(path);
                            socket.Send(asencd.GetBytes(builder.ToString()));
                            break;
                        default:
                            break;
                    }
                    
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
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
        }

        private static StringBuilder ReadFileAttributes(string path)
        {
            Char[] chars = Path.GetInvalidFileNameChars();
            foreach (var item in chars)
            {
                path = path.Replace(item.ToString(), "");
            }

            DirectoryInfo info = new DirectoryInfo(path);
            StringBuilder builder = new StringBuilder();
            foreach (var item in info.GetFiles())
            {
                builder.AppendFormat("{0} : {1} {2}", item.DirectoryName, item.Name, item.Attributes);
                builder.AppendLine("");
            }
            Console.WriteLine(builder.ToString());
            return builder;
        }
    }
}
