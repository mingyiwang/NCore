namespace Core.Http.Enums
{
    public sealed class HttpSchema
    {
        public static readonly HttpSchema Http  = new HttpSchema("http");
        public static readonly HttpSchema Https = new HttpSchema("https");

        private readonly string _type;

        private HttpSchema(string type)
        {
            _type = type;
        }

        public override string ToString()
        {
            return _type;
        }

        public static HttpSchema Of(string schema)
        {
            return schema.ToLower().Equals("https") ? Https : Http;
        }

    }

}