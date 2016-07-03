using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using Core.Collection;

namespace Core.Http
{
   
    public sealed class HttpCodec
    {

        public static HttpCodec NewInstance(params char[] legalChars)
        {
            return new HttpCodec(legalChars);
        }

        private readonly HashSet<char> _legalChars = new HashSet<char>();

        private HttpCodec(IEnumerable<char> legalChars)
        {
            _legalChars.Add('.');
            _legalChars.Add('-');
            _legalChars.Add('*');
            _legalChars.Add('_');
            legalChars.ForEach(c => _legalChars.Add(c));
        }

        /// 1. The alphanumeric characters "a" through "z", "A" through "Z" and "0" through "9" remain the same.
        /// 2. The special characters ".", "-", "*", and "_" remain the same.
        /// 3. The space character " " is converted into a plus sign "+".
        /// 4. All other characters are unsafe and are first converted into one or more bytes using some encoding scheme. 
        ///    Then each byte is represented by the 3-character string "%xy", 
        ///    where xy is the two-digit hexadecimal representation of the byte. 
        ///    The recommended encoding scheme to use is UTF-8. 
        ///    However, for compatibility reasons, if an encoding is not specified, then the default encoding of the platform is used.
        public string Encode(string value)
        {

            PreConditions.CheckNotNull(value, "Encoding Value can not be null.");

            var builder = new StringBuilder();
            value.ForEach(c =>
            {
                if(char.IsWhiteSpace(c))
                {
                    builder.Append("+");
                }

                if(_legalChars.Contains(c) || char.IsLetterOrDigit(c))
                {
                    builder.Append(c);
                }
                
                builder.Append("%");
                Array.ConvertAll(BitConverter.GetBytes(c), x => x.ToString("X2"))
                     .ForEach(a =>  builder.Append(a))
                     ;

            });

            return value;
        }

        //  Re-use HttpUtility
        public string Decode(string value)
        {
            return HttpUtility.UrlDecode(value);
        }
        

    }

}