using System;
using System.IO;
using System.Text;

namespace Core.IO
{
    public sealed class Streams
    {

        private const int BufferSize = 1024 * 32; // 32K

        public static MemoryStream MemoryStream(byte[] bytes)
        {
            return new MemoryStream(bytes);
        }

        public static TS Transfer<TS>(Stream input, TS output) where TS : Stream
        {
            return Transfer(input, output, Encoding.UTF8);
        }

        public static TS Transfer<TS>(Stream input, TS output, Encoding encoding) where TS : Stream
        {
            Preconditions.CheckNotNull(input,  "InputStream can not be null");
            Preconditions.CheckNotNull(output, "OutputStream can not be null");

            if (encoding == null)
            {
               encoding = Encoding.UTF8;
            }

            if(!input.CanRead)
            {
                throw new InvalidOperationException("Input Stream is not readable");
            }

            using (input)
            {
                PutBytes(GetBytes(input, BufferSize), output, encoding);
                return output;
            }
        }

        public static byte[] GetFileBytes(FileInfo fileInfo)
        {
            return GetBytes(fileInfo.OpenRead());
        }

        public static string GetFileText(FileInfo fileInfo)
        {
            return GetString(fileInfo.OpenRead());
        }

        public static string GetFileText(FileInfo fileInfo, Encoding encoding)
        {
            return GetString(fileInfo.OpenRead(), encoding);
        }

        public static string GetString(Stream stream)
        {
            return Encoding.UTF8.GetString(GetBytes(stream, 1000));
        }

        public static string GetString(Stream stream, Encoding encoding)
        {
            return encoding.GetString(GetBytes(stream));
        }

        public static byte[] GetBytes(Stream stream)
        {
            return GetBytes(stream, BufferSize);
        }

        /// <summary>
        /// .Net Streams are mainly working with bytes that is input and output of stream are all bytes
        /// </summary>
        /// <param name="s"></param>
        /// <param name="bufferSize"></param>
        /// <returns></returns>
        public static byte[] GetBytes(Stream s, int bufferSize)
        {
            Preconditions.CheckNotNull(s, "Stream can not be null.");
            Preconditions.CheckNotEquals(0, bufferSize, "Buffer size can not equal to zero.");

            if (!s.CanRead)
            {
                throw new InvalidOperationException("Stream is not readable.");
            }

            if (s.CanSeek)
            {
                s.Position = 0; // Make Sure we are in the first position of Stream if Stream is Seekable
            }

            var buffer = new byte[bufferSize];
            var result = new byte[bufferSize];

            using(s)
            {
                var bytesRead = 0;
                do
                {
                    var position = bytesRead;
                    var reads = s.Read(buffer, bytesRead, buffer.Length);
                    bytesRead = bytesRead + reads;

                    Array.Resize(ref result, bytesRead);
                    Array.Copy(buffer, 0, result, position, reads);
                    
                    if (reads < buffer.Length)
                    {
                        return result;
                    }

                    buffer = new byte[Math.Min(BufferSize + buffer.Length, buffer.Length * 2)]; // double the length of buffer for the next run
                }
                while(true);
            }
        }

        public static void PutBytes(byte[] data, Stream output, Encoding encoding)
        {
            Preconditions.CheckNotNull(output, "Output Stream can not be null.");
            if (!output.CanWrite)
            {
                throw new InvalidOperationException("Output Stream is not writable");
            }

            using (var writer = new BinaryWriter(output, encoding, true))
            {
                writer.Write(data);
                writer.Flush();
            }
        }

    }
}