using System;
using System.IO;
using System.Text;

namespace DFS
{
    public class StorageServer
    {
        string dataDirectory = "/tmp/worker/";

        void Put(string blockUUID, object data)
        {
            FileStream fs = new FileStream(dataDirectory + blockUUID, FileMode.OpenOrCreate);
            fs.Write(Encoding.ASCII.GetBytes(data.ToString()), 0, 0);
        }

        void Get(string blockUUID)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(dataDirectory + blockUUID);
            var files = directoryInfo.GetFiles();
            foreach (var item in files)
            {
                Console.WriteLine("{0} : {1} {2}", item.DirectoryName, item.FullName, item.Extension);
            }
        }
    }
}
