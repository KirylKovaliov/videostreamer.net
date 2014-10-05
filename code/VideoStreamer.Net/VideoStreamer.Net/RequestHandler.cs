using System;
using System.IO;
using System.Runtime.Remoting.Contexts;
using System.Web;
using VideoStreamer.Net.Storage;

namespace VideoStreamer.Net
{
    public class RequestHandler
    {
        #region inner classes
        struct Range<T>
        {
            public Range(T start, T end)
            {
                Start = start;
                End = end;
            }

            public T Start;
            public T End;
        }

        class HeaderInfo
        {
            public Range<long?> Range { get; set; }
        }
        #endregion

        private readonly IStorage _storage;

        public RequestHandler(IStorage storage)
        {
            _storage = storage;
        }

        public void Handle(HttpContext context, string file)
        {
            try
            {
                HandleInternal(context, file);
            }
            catch (FileNotFoundException)
            {
                throw new HttpException(404, "Unable to load file fom remote storage");
            }
        }

        private void HandleInternal(HttpContext context, string file)
        {
            long size = _storage.GetLength(file);
            long end = size - 1;

            context.Response.AddHeader("Accept-Ranges", string.Format("0-" + size));
            var headerInfo = ReadRequestHeader(context.Request);
            if (!IsRequestedRangeValid(headerInfo.Range, end))
            {
                context.Response.AddHeader("Content-Range", "bytes " + 0 + "-" + end + "/" + size);
                throw new HttpException(416, "Requested Range Not Satisfiable");
            }

            var actualStart = headerInfo.Range.Start ?? 0;
            var actualEnd = headerInfo.Range.End ?? end;
            var length = actualEnd - actualStart + 1;

            //var position =  reader.BaseStream.Seek(actualStart, SeekOrigin.Begin);
            var buffer = _storage.Read(file, actualStart, length);
            context.Response.StatusCode = 206;
            context.Response.AddHeader("Content-Range", "bytes " + actualStart + "-" + actualEnd + "/" + size);
            context.Response.AddHeader("Content-Length", length.ToString());
            context.Response.BinaryWrite(buffer);
            context.Response.End();
        }

        private HeaderInfo ReadRequestHeader(HttpRequest request)
        {
            Range<long?> range = new Range<long?>();
            if (!string.IsNullOrEmpty(request.Headers["Range"]))
            {
                string rangeHeader = request.Headers["Range"].Replace("bytes=", string.Empty);
                string[] splittedRangeHeader = rangeHeader.Split('-');
                range.Start = splittedRangeHeader.Length > 0 ? ToNullable(splittedRangeHeader[0]) : null;
                range.End = splittedRangeHeader.Length > 1 ? ToNullable(splittedRangeHeader[1]) : null;
            }

            return new HeaderInfo
            {
                Range = range
            };
        }

        private bool IsRequestedRangeValid(Range<long?> range, long end)
        {
            if (range.Start.HasValue && range.Start.Value > end)
                return false;

            if (range.Start.HasValue && range.Start.Value < 0)
                return false;

            if (range.End.HasValue && range.End.Value > end)
                return false;

            if (range.End.HasValue && range.End.Value <= 0)
                return false;

            return true;
        }

        private long? ToNullable(string str)
        {
            long result;

            return long.TryParse(str, out result) ? (long?)result : null;
        }
    }
}
