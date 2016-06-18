
using System.Collections.Generic;

namespace Core.Http.Entities
{
    public abstract class HttpEntity
    {

        public const string UrlEncodedForm    = "application/x-www-form-urlencoded";
        public const string MultipartFormData = "application/form-data";
        public const string TextPlain         = "application/text-plain";
        public const string Json              = "application/json";

        public virtual bool IsUrlEncodedFormEntity() { return false; }
        public virtual bool IsTextEntity()           { return false; }
        public virtual bool IsFormDataEntity()       { return false; }
        public virtual MultipartFormDataEntity AsMultipartFormData() { return null;}
        public virtual UrlEncodedFormEntity    AsUrlEncodingForm()   { return null;}
        public virtual TextEntity              AsText()              { return null;}

        public abstract string ContentType { get; }
        protected abstract byte[] CreateBody();
        private byte[] _content;

        public byte[] GetBody()
        {
            return _content ?? (_content = CreateBody());
        }

        public int ContentLength => GetBody().Length;

        public static TextEntity Text(string text)
        {
            return new TextEntity(text);
        }

        public static UrlEncodedFormEntity UrlEncoded()
        {
            return new UrlEncodedFormEntity();
        }

        public static UrlEncodedFormEntity UrlEncoded(IEnumerable<KeyValuePair<string, string>> parameters)
        {
            return new UrlEncodedFormEntity().AddParameters(parameters);
        }

        public static MultipartFormDataEntity Multipart()
        {
            return new MultipartFormDataEntity();
        }

        public static MultipartFormDataEntity Multipart(IEnumerable<KeyValuePair<string, string>> parameters)
        {
            return new MultipartFormDataEntity().AddParameters(parameters);
        }

    }
}