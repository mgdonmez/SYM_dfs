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
                while (true)
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
                    string[] arguements = message.Replace("\0", "").ToString().Split('-');
                    string path = "";
                    StringBuilder builder = new StringBuilder();
                    switch (arguements[0])
                    {
                        case "LST":
                            path = arguements[1];
                            builder = ReadFileAttributes(path);
                            socket.Send(asencd.GetBytes(builder.ToString()));
                            break;
                        case "HLP":
                            builder.AppendLine("LST: List files in target path.");
                            builder.AppendLine("CRT: Create new subdirectory in given path.");
                            builder.AppendLine("CRTF: Create new file in given path.");
                            builder.AppendLine("WRT: Write given message to file.");
                            builder.AppendLine("RD: Read the content of the file.");
                            builder.AppendLine("DEL: Delete the file/directory.");
                            builder.AppendLine("HLP: List this message you dummy.");
                            socket.Send(asencd.GetBytes(builder.ToString()));
                            break;
                        case "CRT":
                            path = arguements[1];
                            CreateSubDirectory(path);
                            if (new DirectoryInfo(@"Storage\" + path).Exists)
                                socket.Send(asencd.GetBytes("Subdirectory created"));
                            else
                                socket.Send(asencd.GetBytes("Subdirectory create failed."));
                            break;
                        case "CRTF":
                            path = arguements[1];
                            CreateSubDirectory(path);
                            if (new DirectoryInfo(@"Storage\" + path).Exists)
                            {
                                File.Create(@"Storage\" + path + @"\" + arguements[2]);
                            }
                            socket.Send(asencd.GetBytes("File create successful"));
                            break;
                        case "WRT":
                            path = arguements[1];
                            CreateSubDirectory(path);
                            if (new DirectoryInfo(@"Storage\" + path).Exists)
                            {
                                if (File.Exists(@"Storage\" + path + @"\" + arguements[2]))
                                {
                                    File.WriteAllText(@"Storage\" + path + @"\" + arguements[2], arguements[3]);
                                    socket.Send(asencd.GetBytes("Write completed."));
                                }
                                else
                                    socket.Send(asencd.GetBytes("File not found."));
                            }
                            break;
                        case "RD":
                            path = arguements[1];
                            CreateSubDirectory(path);
                            if (new DirectoryInfo(@"Storage\" + path).Exists)
                            {
                                if (File.Exists(@"Storage\" + path + @"\" + arguements[2]))
                                {
                                    var content = File.ReadAllText(@"Storage\" + path + @"\" + arguements[2]);
                                    socket.Send(asencd.GetBytes(content));
                                }
                                else
                                    socket.Send(asencd.GetBytes("File not found."));
                            }
                            break;
                        case "DEL":
                            path = arguements[1];
                            var info = new DirectoryInfo(@"Storage\" + path);
                            if (info.Exists)
                            {
                                info.Delete();
                                socket.Send(asencd.GetBytes("Delete completed."));
                            }
                            break;
                        default:
                            builder.AppendLine("LST: List files in target path.");
                            builder.AppendLine("CRT: Create new subdirectory in given path.");
                            builder.AppendLine("CRTF: Create new file in given path.");
                            builder.AppendLine("WRT: Write given message to file.");
                            builder.AppendLine("RD: Read the content of the file.");
                            builder.AppendLine("DEL: Delete the file/directory.");
                            builder.AppendLine("HLP: List this message you dummy.");
                            socket.Send(asencd.GetBytes(builder.ToString()));
                            break;
                    }

                    socket.Send(asencd.GetBytes("Automatic Message:" + "String Received byte server !"));
                    Console.WriteLine("\nAutomatic Message is Sent");
                    socket.Close();
                    mylist.Stop();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error.." + ex.StackTrace);
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
        }

        private static void CreateSubDirectory(string path)
        {
            DirectoryInfo info = new DirectoryInfo(@"Storage\");
            info.CreateSubdirectory(path);
        }

        private static StringBuilder ReadFileAttributes(string path)
        {
            DirectoryInfo info = new DirectoryInfo(path);
            StringBuilder builder = new StringBuilder();
            foreach (var item in info.GetFiles())
            {
                builder.AppendFormat("{0} : {1} {2}", item.DirectoryName, item.Name, item.Attributes);
                builder.AppendLine("");
            }
            return builder;
        }
    }
}
