using Com.Hrm123.Nextgenvideo;
using Google.Protobuf;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Com.Hrm123.Nextgenvideo.NextGenVideoService;

namespace NextVideoClient
{
    public class NextGenVideoClient
    {
        public int Port { get; set; }
        public string Host { get; set; }
        Channel _channel = null;
        NextGenVideoServiceClient _client = null;
        string _fileName = string.Empty;
        FileStream destinationStream = null;

        public NextGenVideoClient(int port, string host)
        {
            Port = port;
            Host = host;
        }

        public void Start()
        {
            _channel = new Channel(Host + ":" + Port, ChannelCredentials.Insecure);
            _client = new NextGenVideoServiceClient(_channel);
        }

        public async Task<Tuple<bool, string>> GetFile(string fileName, string fileExt)
        {
            FileReq req = new FileReq
            {
                Fullfilename = fileName + fileExt
            };

            string fileNameLocal = "c:\\temp\\" + fileName + "_"
                    + System.Guid.NewGuid().ToString() + fileExt;

            if (destinationStream == null)
            {
                //create file to save stream to
                destinationStream = File.Create(fileNameLocal );
            }

            var stream = new CodedOutputStream(destinationStream, true);

            using(var call = _client.GetFile(req))
            {
                while(await call.ResponseStream.MoveNext())
                {
                    var arr = call.ResponseStream.Current.PayLoad.ToByteArray();
                    destinationStream.Write(arr, 0, arr.Length);
                }
            }
            await destinationStream.FlushAsync();
            stream.Flush();
            stream.Dispose();

            if(destinationStream != null)
            {
                destinationStream.Dispose();
                destinationStream.Close();
                destinationStream = null;
            }
            return new Tuple<bool, string>( true, fileNameLocal);
        }


    }
}
