using System;
using System.IO;
using System.Text;

namespace Core.IO
{
    public sealed class Streams
    {

        private const int BufferSize = 1024 * 32; // 32K

        public static MemoryStream Empty => new MemoryStream();

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
            Checking.CheckNotNull(input,  "InputStream can not be null");
            Checking.CheckNotNull(output, "OutputStream can not be null");

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
            Checking.CheckNotNull(s, "Stream is null, please make sure Stream is reachable or the resource is Embedded Resource");
            Checking.CheckNotEquals(0, bufferSize, "Buffer size must not be 0 or negative number.");

            if (!s.CanRead)
            {
                throw new InvalidOperationException("Stream is not readable.");
            }

            if (s.CanSeek)
            {
                s.Position = 0; // Make Sure we are in the first position of Stream if Stream is Seekable
            }
            
            using (var reader = new BinaryReader(s))
            {
                return reader.ReadBytes(BufferSize);
            }
            
        }

        public static void PutBytes(byte[] data, Stream output, Encoding encoding)
        {
            Checking.CheckNotEmpty(data , "Collection can not be empty");
            Checking.CheckNotNull(output, "Output Stream can not be null.");
            
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