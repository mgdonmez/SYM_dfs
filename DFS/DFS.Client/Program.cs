using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DFS.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                TcpClient Tcpclient = new TcpClient();
                Console.WriteLine("Connecting..");
                Tcpclient.Connect("127.0.0.1", 8000);
                Console.WriteLine("Connected");
                Console.WriteLine("Enter the command you want to send.");
                Console.WriteLine("HLP for help.");
                string str = Console.ReadLine();
                Stream stm = Tcpclient.GetStream();
                ASCIIEncoding ascnd = new ASCIIEncoding();
                byte[] ba = ascnd.GetBytes(str);
                Console.WriteLine("Sending command.");
                
                stm.Write(ba, 0, ba.Length);
                byte[] bb = new byte[1000];
                int k = stm.Read(bb, 0, 1000);
                for (int i = 0; i < k; i++)
                {
                    Console.Write(Convert.ToChar(bb[i]));
                }

                Tcpclient.Close();
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error" + ex.StackTrace);
            }
        }
    }
}
