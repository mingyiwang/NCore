using System.Collections.Generic;
using System.Text;
using Core.Collection;
using Core.Primitive;

namespace Core.Http.Entities
{

    public sealed class UrlEncodedFormEntity : HttpEntity
    {

        public static readonly HttpCodec ENCODER = HttpCodec.NewInstance('&','=');
        private readonly HashMap<string, string> _parameters = new HashMap<string, string>();

        public override string ContentType => UrlEncodedForm;

        public UrlEncodedFormEntity AddParameter<T>(string key, T value)
        {
            _parameters.Put(key, Strings.Of(value));
            return this;
        }

        public UrlEncodedFormEntity AddParameters<T>(IEnumerable<KeyValuePair<string, T>> parameters)
        {
            foreach(var parameter in parameters)
            {
                _parameters.Put(parameter.Key, Strings.Of(parameter.Value));
            }
            return this;
        }

        protected override byte[] CreateBody()
        {
            var content = Collections.Join('&', _parameters, pair => pair.Key + "=" + pair.Value);
            return Encoding.UTF8.GetBytes(ENCODER.Encode(content));
        }

        public override bool IsUrlEncodedFormEntity()
        {
            return true;
        }

        public override UrlEncodedFormEntity AsUrlEncodingForm()
        {
            return this;
        }

    }

}