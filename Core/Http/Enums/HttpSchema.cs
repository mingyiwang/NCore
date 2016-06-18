namespace Core.Http.Enums
{
    public sealed class HttpSchema
    {
        public readonly static HttpSchema HTTP  = new HttpSchema("http");
        public readonly static HttpSchema HTTPS = new HttpSchema("https");

        private readonly string _type;

        private HttpSchema(string type)
        {
            _type = type;
        }

        public string GetSchema()
        {
            return _type;
        }

        public static HttpSchema GetSchema(string schema)
        {
            return schema.ToLower().Equals("https") ? HTTPS : HTTP;
        }
    }

}