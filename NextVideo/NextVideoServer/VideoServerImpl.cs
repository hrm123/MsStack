using Com.Hrm123.Nextgenvideo;
using Google.Protobuf;
using Grpc.Core;
using Grpc.Core.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NextVideoServer
{
    public class VideoServerImpl : NextGenVideoService.NextGenVideoServiceBase
    {
        FileStream destinationStrem = null;

        public VideoServerImpl()
        {
            GrpcEnvironment.SetLogger(new TextWriterLogger(Console.Out));
        }

        public override Task<FileListResp> ListFiles(FileListReq request, ServerCallContext context)
        {
            DirectoryInfo DirInfo = new DirectoryInfo(@"c:\temp\");
            var files = from f in DirInfo.EnumerateFiles()
                        where f.Extension == ".mp4"
                        orderby f.CreationTime
                        select new FileName
                        {
                            FileName_ = f.Name
                        };
            var resp = new FileListResp();
            resp.Files.AddRange(files);
            
            return Task.FromResult(resp);
        }

        public override async Task GetFile(FileReq request, IServerStreamWriter<Chunk> responseStream, ServerCallContext context)
        {
            long dataToRead;
            byte[] buffer = new Byte[4096];
            int length;
            int bytesRead = 0;
            Chunk msg = new Chunk();

            try
            {
                Stream inpStream = new FileStream("c:\\temp\\" + request.Fullfilename, FileMode.Open,
                    FileAccess.Read, FileShare.Read);
                dataToRead = inpStream.Length;
                int start = 0;
                inpStream.Seek(start, SeekOrigin.Begin);
                while(dataToRead > 0)
                {
                    // Read the data in buffer
                    length = inpStream.Read(buffer, 0, buffer.Length);

                    // Write the data to the current output stream
                    msg.PayLoad = ByteString.CopyFrom(buffer, 0, buffer.Length);
                    await responseStream.WriteAsync(msg);
                    bytesRead += buffer.Length;

                    buffer = new Byte[buffer.Length];
                    dataToRead = dataToRead - buffer.Length;
                }
            }
            catch(Exception ex)
            {
                string str = ex.ToString();
            }
        }

        public override async Task<SvcResponse> SaveMp4File(IAsyncStreamReader<Chunk> requestStream, ServerCallContext context)
        {
            String fileName = System.Guid.NewGuid().ToString() + ".mp4";
            try
            {
                SvcResponse resp = new SvcResponse();

                resp.Message = fileName;

                Console.WriteLine("SaveFile ->" + fileName);

                CancellationToken ct = new CancellationToken();

                if (destinationStrem == null)
                {
                    destinationStrem = File.Create("c:\\temp\\" + fileName);
                }

                var stream = new CodedOutputStream(destinationStrem, true);
                while (await requestStream.MoveNext(ct))
                {
                    var arr = requestStream.Current.PayLoad.ToByteArray();
                    destinationStrem.Write(arr, 0, arr.Length);
                }
                destinationStrem.Flush();
                stream.Flush();
                stream.Dispose();

                if (destinationStrem != null)
                {
                    destinationStrem.Dispose();
                    destinationStrem.Close();
                    destinationStrem = null;
                }
                Console.WriteLine("saved file -" + fileName);

                return new SvcResponse
                {
                    Message = fileName,
                    Code = "1"
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new SvcResponse
                {
                    Message = fileName,
                    Code = "0"
                };
            }
        }
    }
}
