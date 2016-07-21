using System.Text;
using Core.Concurrent;

namespace Core.Security
{

    public sealed class Encoder
    {

        public static readonly Encoding ASCII = Encoding.ASCII;
        public static readonly Encoding UTF8  = Encoding.UTF8;
        public static readonly Encoding UTF16BigEndian    = Encoding.BigEndianUnicode;
        public static readonly Encoding UTF16LitterEndian = Encoding.Unicode;
        public static readonly Encoding UTF16 = OS.IsLittleEndian ? UTF16LitterEndian : UTF16BigEndian;

        public static string GetString(byte[] dataBytes, Encoding encoding)
        {
            var bytes = Encoding.Convert(EncodingOf(dataBytes), encoding, dataBytes);
            return encoding.GetString(bytes);
        }

        public static byte[] GetBytes(string data, Encoding encoding)
        {
            return encoding.GetBytes(data);
        }

        public static Encoding EncodingOf(byte[] dataBytes)
        {
            return UTF8;
        }

    }

}