using System;
using System.Collections.Generic;
using System.Net;
using Core.Collection;

namespace Core.Http.Enums
{
    public class HttpCookies : IEquatable<HttpCookies>
    {

        private readonly HashMap<string, Cookie> _cookies = new HashMap<string, Cookie>();
        public bool IsEmpty => _cookies.Size() == 0;

        public IReadOnlyDictionary<string, Cookie> AsReadOnly()
        {
            return Collections.AsReadOnly(_cookies);
        }

        public HttpCookies AddCookie(Cookie cookie)
        {
            _cookies.Add(cookie.Name, cookie);
            return this;
        }

        public HttpCookies AddCookies(HttpCookies cookies)
        {
            if (!cookies.IsEmpty)
            {
                foreach(var pair in cookies.AsReadOnly())
                {
                    _cookies.Add(pair.Key, pair.Value);
                }
            }
            return this;
        }

        public HttpCookies AddCookies(CookieCollection collection)
        {
            if (collection.Count > 0)
            {
                for(int i = 0; i < collection.Count; i++)
                {
                    _cookies.Add(collection[i].Name, collection[i]);
                }
            }
            return this;
        }

        public Cookie GetCookie(string name)
        {
            return _cookies.Get(name);
        }

        public bool Equals(HttpCookies other)
        {
            return Collections.Equals(_cookies, other._cookies);
        }

    }

}