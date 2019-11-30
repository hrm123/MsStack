using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextVideoServer
{
    class Program
    {
        static void Main(string[] args)
        {
            int port = 50051;
            string host = "localhost";
            
            if(args.Length == 2)
            {
                if(!int.TryParse(args[0], out port))
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

            VidServer srvr = new VidServer(port, host);
            srvr.Start();

            Console.WriteLine("Server started on " + host + ":" + port);
            Console.WriteLine("Press any key to stop the server ...");

            Console.ReadKey();

        }
    }
}
