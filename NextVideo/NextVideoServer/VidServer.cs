using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;
using Com.Hrm123.Nextgenvideo;

namespace NextVideoServer
{
    public class VidServer
    {
        public int Port { get; set; }
        public string Host { get; set; }

        Server _server = null;


        public VidServer(int port, string host)
        {
            Port = port;
            Host = host;
        }

        public void Start()
        {
            _server = new Server
            {
                Services = { NextGenVideoService.BindService(new VideoServerImpl()) },
                Ports = { new ServerPort(Host, Port, ServerCredentials.Insecure) }
            };
            _server.Start();
        }

        public void Stop()
        {
            if(_server != null)
            {
                _server.ShutdownAsync().Wait();
                _server = null;
            }
        }


    }
}
