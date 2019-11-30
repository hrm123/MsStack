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
        FileStream sourceStream = null;

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


        public async Task<bool> SaveFile(string fileName, string fileExt)
        {
            try
            {
                if (fileExt != ".mp4")
                {
                    throw new ApplicationException("not implemented.");
                }

                string fileNameLocal = "c:\\temp\\" + fileName + fileExt;
                if (sourceStream == null)
                {
                    //create file to save stream to
                    sourceStream = File.OpenRead(fileNameLocal);
                }
                byte[] buffer = new byte[1024];
                int totalLen = 0;
                using (var call = _client.SaveMp4File())
                {
                    int bytesRead = sourceStream.Read(buffer, 0, buffer.Length);

                    while (bytesRead > 0)
                    {
                        Chunk msg = new Chunk();
                        totalLen += buffer.Length;
                        msg.PayLoad = ByteString.CopyFrom(buffer, 0, buffer.Length);
                        await call.RequestStream.WriteAsync(msg);
                        bytesRead = sourceStream.Read(buffer, 0, buffer.Length);
                        Console.Write("chunk sent..");
                    }
                    await call.RequestStream.CompleteAsync();
                    var resp = await call.ResponseAsync;
                    if (resp.Code == "1")
                    {
                        Console.Write("sent the chunks of file -" + fileNameLocal);
                    }
                    else
                    {
                        Console.Write("error in sending  the chunks of file -" + fileNameLocal);
                    }
                }
                return true;
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
                return false;
            }

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

        public async Task<Tuple<bool, string[]>> ListFiles()
        {

            FileListReq req = new FileListReq();
            FileListResp resp = await _client.ListFilesAsync(req);
            return new Tuple<bool, string[]>(true, resp.Files.Select((v, i) => v.FileName_).ToArray());
        }


    }
}
