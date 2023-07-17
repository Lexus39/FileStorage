using System.Net;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace FileStorage.Client
{
    public class ProgressStreamContent : StreamContent
    {
        private readonly Stream _stream;
        private readonly int _bufferLength = 1024 * 20;

        public ProgressStreamContent(Stream stream, 
            int bufferLength, Action<long> onProgress) : base(stream)
        {
            _stream = stream;
            _bufferLength = bufferLength;
            OnProgress += onProgress;
        }

        public event Action<long> OnProgress;

        protected async override Task SerializeToStreamAsync(Stream stream, TransportContext? context)
        {
            var buffer = new byte[_bufferLength];
            long uploaded = 0;
            while (true)
            {
                using (_stream)
                {
                    var length = await _stream.ReadAsync(buffer, 0, _bufferLength);
                    if (length <= 0)
                        break;
                    uploaded += length;
                    await stream.WriteAsync(buffer);
                    OnProgress.Invoke(uploaded);
                    await Task.Delay(250);
                }
            }
        }
    }
}
