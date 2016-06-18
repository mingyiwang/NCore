using System;
using System.Net;
using System.Text;
using Core.Collection;
using Core.Http.Enums;

namespace Core.Http
{

    public sealed class HttpResponse
    {
        public bool        Success      { get; set; }
        public byte[]      Response     { get; set; }
        public int         StatusCode   { get; set; }
        public Exception   Error   { get; set; }

        public HttpHeaders Headers { get; }
        public HttpCookies Cookies { get; }

        public HttpResponse() : this(null, null) {}

        public HttpResponse(CookieCollection container, WebHeaderCollection headers)
        {
            Response = Arrays.EmptyBytes;
            Cookies  = new HttpCookies();
            Headers  = new HttpHeaders();

            if (headers != null)
            {
                PutHeaders(headers);
            }

            if (container != null)
            {
                PutCookies(container);
            }
        }

        public bool HasCookies => Cookies.AsReadOnly().Count > 0;
        public bool HasHeaders => !Headers.IsEmpty;

        public Cookie GetCookie(string name)
        {
            return Cookies.GetCookie(name);
        }

        public string GetTextContent()
        {
            return Encoding.UTF8.GetString(Response);
        }

        public string GetTextContent(Encoding encoding)
        {
            return encoding.GetString(Response);
        }

        public HttpResponse PutCookies(CookieCollection collection)
        {
            Cookies.AddCookies(collection);
            return this;
        }
        
        public HttpResponse PutHeaders(WebHeaderCollection collection)
        {
            Headers.AddHeaders(collection);
            return this;
        }

    }

}