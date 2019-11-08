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

            NextGenVideoClient clnt = new NextGenVideoClient(port, host);
            clnt.Start();

            Console.WriteLine("fetching vid1.mp4");
            var resp = clnt.GetFile("vid1", ".mp4");
            resp.Wait();

            Console.WriteLine("file " + resp.Result.Item2 + " fetched sucessfully");
            Console.WriteLine();
        }
    }
}
