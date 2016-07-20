using System;
using System.Text;
using Core.Concurrent;

namespace Core.Security
{
    public class Encodings
    {

        public static readonly Encoding ASCII = Encoding.ASCII;
        public static readonly Encoding UTF8  = Encoding.UTF8;
        public static readonly Encoding UTF16BigEndian = Encoding.BigEndianUnicode;
        public static readonly Encoding UTF16LitterEndian = Encoding.Unicode;
        public static readonly Encoding UTF16 = OS.IsLittleEndian ? UTF16LitterEndian : UTF16BigEndian;

        public static string StringOf(byte[] dataBytes, Encoding encoding)
        {
            var bytes = Encoding.Convert(GetEncoding(dataBytes), encoding, dataBytes);
            return encoding.GetString(bytes);
        }

        public static byte[] BytesOf(string data, Encoding encoding)
        {
            return encoding.GetBytes(data);
        }

        public static Encoding GetEncoding(byte[] dataBytes)
        {
           return UTF8;
        }

    }
}