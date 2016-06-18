using System;
using System.Collections.Generic;
using System.Text;
using Core.Collection;
using Core.Primitive;

namespace Core.Http.Entities
{

    public sealed class MultipartFormDataEntity : HttpEntity
    {

        public override string ContentType => MultipartFormData + ";" + "boundary=" + _boundary;

        private readonly string _boundary;
        private readonly HashMap<string, byte[]> _files = new HashMap<string, byte[]>(); 
        private readonly HashMap<string, string> _parameters = new HashMap<string, string>();

        public MultipartFormDataEntity()
        {
            _boundary = CreateBoundary();
        }

        public MultipartFormDataEntity AddBinary(string key, byte[] binary)
        {
            _files.Put(key, binary);
            return this;
        }

        public MultipartFormDataEntity AddParameters<T>(IEnumerable<KeyValuePair<string, T>> parameters)
        {
            foreach (var parameter in parameters)
            {
                _parameters.Put(parameter.Key, Strings.Of(parameter.Value));
            }
            return this;
        }

        public MultipartFormDataEntity AddParameter<T>(string key, T value)
        {
            _parameters.Put(key, Strings.Of(value));
            return this;
        }

        protected override byte[] CreateBody()
        {
            var builder  = new StringBuilder();
            foreach (var parameter in _parameters)
            {
                builder.AppendLine("--" + _boundary);
                builder.AppendLine($"Content-Disposition: form-data; name=\"{parameter.Key}\"");
                builder.AppendLine(parameter.Value);
            }

            foreach (var file in _files)
            {
                builder.AppendLine("--" + _boundary);
                builder.AppendLine($"Content-Disposition: form-data; Content-Transfer-Encoding : Base64; name=\"{file.Key}\" filename=\"{file.Key}\"");
                builder.AppendLine(Convert.ToBase64String(file.Value));
            }

            builder.AppendLine("--" + _boundary + "--");
            return Encoding.UTF8.GetBytes(builder.ToString());
        }

        private static readonly char[] MultipartChars = 
            "-_1234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

        private static string CreateBoundary()
        {
            var buffer = new StringBuilder();
            var rand   = new Random();
            var count  = rand.Next(11) + 30;
            for (var i = 0; i < count; ++i)
            {
                buffer.Append(MultipartChars[rand.Next(MultipartChars.Length)]);
            }

            return buffer.ToString();
        }

    }

}