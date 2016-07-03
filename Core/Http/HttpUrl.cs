using System;
using System.Text;
using Core.Collection;
using Core.Http.Enums;
using Core.Primitive;

namespace Core.Http
{
    public sealed class HttpUrl
    {

        public static HttpCodec PathEncoding  = HttpCodec.NewInstance(Arrays.Empty<char>());
        public static HttpCodec HostEncoding  = HttpCodec.NewInstance(Arrays.Empty<char>());
        public static HttpCodec QueryEncoding = HttpCodec.NewInstance(Arrays.Empty<char>());
        
        public HashMap<string, string> Query { get; }
        public HttpSchema Schema { get; private set; }
        public string Host { get; private set; }
        public string Path { get; private set; }
        public string Hash { get; private set; }
        public int    Port { get; private set; }
        public Uri    Uri  { get; private set; }

        public HttpUrl()
        {
            Schema = HttpSchema.HTTP;
            Query  = new HashMap<string, string>();
            Host = string.Empty;
            Hash = string.Empty;
            Path = string.Empty;
            Hash = string.Empty;
            Port = 0;
            Uri = null;
        }

        public HttpUrl WithSchema(HttpSchema schema)
        {
            Schema = schema;
            return this;
        }

        public HttpUrl WithHost(string host)
        {
            Host = HostEncoding.Encode(host);
            return this;
        }

        /// <summary>
        /// Path can start with '/' or not
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public HttpUrl WithPath(string path)
        {
            Path = PathEncoding.Encode(path);
            return this;
        }

        public HttpUrl WithPort(int port)
        {
            Port = port;
            return this;
        }

        public HttpUrl WithHash(string hash)
        {
            Hash = hash;
            return this;
        }

        public HttpUrl WithParameter<T>(string name, T value)
        {
            if (!string.IsNullOrEmpty(name) && value != null)
            {
                 Query.Add(name, Strings.Of(value));
            }
            return this;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append(Schema.GetSchema() + "://")
                   .Append(HostEncoding.Encode(HostEncoding.Encode(Host.Trim('/'))))
                   .Append("/")
                   .Append(PathEncoding.Encode(PathEncoding.Encode(Path.Trim('/'))));

            if(Query.Size() > 0)
            {
                builder.Append("?")
                       .Append(Collections.Join('&', Query, pair => QueryEncoding.Encode(pair.Key) + "=" + QueryEncoding.Encode(pair.Value)));
            }

            if(!string.IsNullOrEmpty(Hash))
            {
                builder.Append("#" + PathEncoding.Encode(Hash));
            }

            return builder.ToString();
        }

        public Uri Build()
        {
            PreConditions.CheckNotBlank(Host, "Host can not be empty");
            PreConditions.CheckNotBlank(Path, "Path can not be empty");
            return Uri = new Uri(ToString());
        }

        public static HttpUrl From(string url)
        {
            PreConditions.CheckNotBlank(url, "Url can not be empty.");

            Uri uri;
            var succeed = Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out uri);
            if (!succeed || string.IsNullOrEmpty(uri.Scheme))
            {
                throw new UriFormatException("Url is not well formed.");
            }

            var httpUrl = new HttpUrl
            {
                Uri    = uri,
                Schema = HttpSchema.GetSchema(uri.Scheme),
                Hash   = Strings.Of(uri.Fragment),
                Host   = Strings.Of(uri.Host),
                Path   = Strings.Of(uri.AbsolutePath),
            };

            if (!string.IsNullOrEmpty(uri.Query))
            {
                var entires = uri.Query.Split('&');
                foreach (var entry in entires)
                {
                    var items = entry.Split('=');
                    httpUrl.Query.Put(
                        QueryEncoding.Decode(Strings.Of(items[0])), 
                        QueryEncoding.Decode(Strings.Of(items[1]))
                    );
                }
            }

            return httpUrl;
        }
        
    }
}