using System.Text;
using Core.Primitive;

namespace Core.Http.Entities
{

    public sealed class TextEntity : HttpEntity
    {

        public string Content
        {
            get;
            private set;
        }

        public override string ContentType => TextPlain;

        public TextEntity(string text)
        {
            Content = text;
        }

        public TextEntity WithContent(string content)
        {
            Content = content;
            return this;
        }

        protected override byte[] CreateBody()
        {
            return Encoding.UTF8.GetBytes(HttpCodec.Of().Encode(Strings.Of(Content)));
        }

    }

}