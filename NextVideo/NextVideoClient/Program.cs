using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextVideoClient
{
    public class Program
    {
        static void Main(string[] args)
        {
            int port = 50051;
            string host = "localhost";
            string mode = "1";

            if (args.Length == 3)
            {
                if (!int.TryParse(args[0], out port))
                {
                    port = 50051;
                }

                if (!String.IsNullOrEmpty(args[1]))
                {
                    host = args[1];
                }

                if (!String.IsNullOrEmpty(args[2]))
                {
                    mode = args[2]; // can be "list" or "<filename without extension>"
                }
            }
            else
            {
                if (args.Length == 2)
                {
                    if (!int.TryParse(args[0], out port))
                    {
                        port = 50051;
                    }

                    if (!String.IsNullOrEmpty(args[1]))
                    {
                        host = args[1];
                    }
                }
                else
                {
                    Console.WriteLine("Arguments not correct.. using default port and host");
                }
            }

            NextGenVideoClient clnt = new NextGenVideoClient(port, host);
            clnt.Start();
            if (mode == "1")
            {
                Console.WriteLine("fetching vid1.mp4");
                var resp = clnt.GetFile("vid1", ".mp4");
                resp.Wait();
                Console.WriteLine("file " + resp.Result.Item2 + " fetched sucessfully");
            }
            else if(mode == "list")
            {
                var resp = clnt.ListFiles();
                resp.Wait();
                string fileString = string.Join(",", resp.Result.Item2);
                Console.WriteLine("file list - " + fileString);
            }
            else
            {
                //should be file name
                Console.WriteLine("saving " + mode + ".mp4");
                var resp = clnt.SaveFile(mode, ".mp4");
                resp.Wait();
                if (resp.Result)
                {
                    Console.WriteLine("file " + mode + ".mp4" + " saved sucessfully");
                }
                else
                {
                    Console.WriteLine("file " + mode + ".mp4" + " save faield");
                }
            }
            
            Console.WriteLine();
        }
    }
}
