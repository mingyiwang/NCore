using System;
using System.Net;
using Core.Http.Entities;
using Core.Http.Enums;
using Core.Http.Impl;
using Core.Primitive;

namespace Core.Http
{

    /// <summary>
    /// HttpChannel is designed to simplify Usage of HttpWebReqest in special case of Get and Post
    /// </summary>
    public abstract class HttpChannel
    {

        public bool   AllowCookies  { get; private set; }
        public bool   AllowRedirect { get; private set; }
        public bool   KeepAlive     { get; private set; }
        public string Accept        { get; private set; }
        public string Connection    { get; private set; }
        public string UserAgent     { get; private set; }

        public HttpAuthentication Authentication { get; private set; }
        public HttpUrl     HttpUrl  { get; private set; }
        public HttpEntity  Entity   { get; private set; }

        public HttpHeaders Headers  { get; private set; }
        public HttpCookies Cookies  { get; private set; }

        public abstract HttpResponse Send();
        public abstract HttpResponse Get();

        public HttpResponse Send(HttpEntity entity)
        {
            return WithEntity(entity).Send();
        }

        public void SendAsync(Action<HttpResponse> callback)
        {
            callback(Send());
        }

        public void SendAsync(HttpEntity entity, Action<HttpResponse> callback)
        {
            callback(Send(entity));
        }

        public void GetAsync(Action<HttpResponse> callback)
        {
            callback(Get());
        }

        public static HttpChannel NewChannel(string url)
        {
            return new DefaultHttpChannelImpl
            {
                HttpUrl        = HttpUrl.From(url),
                Headers        = new HttpHeaders(),
                Cookies        = new HttpCookies(),
                Authentication = null,
                Entity         = null,
                AllowCookies   = true,
                AllowRedirect  = true,
                KeepAlive      = true
            };
        }

        /// <summary>
        /// Prepare Url
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static HttpChannel NewChannel(HttpUrl url)
        {
            return new DefaultHttpChannelImpl
            {
                HttpUrl        = url,
                Authentication = null,
                Entity         = null,
                AllowCookies   = true,
                AllowRedirect  = true,
                KeepAlive      = true
            };
        }

        public HttpChannel WithAccept(string accept)
        {
            Accept = accept;
            return this;
        }

        public HttpChannel WithConnection(string connection)
        {
            Connection = connection;
            return this;
        }

        public HttpChannel WithUserAgent(string userAgent)
        {
            UserAgent = userAgent;
            return this;
        }

        public HttpChannel WithCookie(Cookie cookie)
        {
            if (!AllowCookies)
            {
                return this;
            }

            if (cookie == null)
            {
                return this;
            }

            cookie.Domain   = Strings.Of(HttpUrl.Host);// make sure domain is set
            cookie.Path     = Strings.Of(HttpUrl.Path);// make sure path is set
            cookie.Secure   = true;
            cookie.HttpOnly = true;
            Cookies.AddCookie(cookie);
            return this;
        }

        public HttpChannel WithCookie<T>(string name, T value)
        {
            if (!string.IsNullOrEmpty(name))
            {
                WithCookie(new Cookie(name, Strings.Of(value)));
            }

            return this;
        }

        public HttpChannel WithHeader<T>(HttpRequestHeader name, T value)
        {
            Headers.AddHeader(name, Strings.Of(value));
            return this;
        }

        public HttpChannel WithHeader<T>(string name, T value)
        {
            Headers.AddHeader(name, Strings.Of(value));
            return this;
        }

        public HttpChannel WithHeaders(HttpHeaders headers, bool createNew)
        {
            if (createNew)
            {
                Headers = headers;
            }
            else
            {
                Headers.AddHeaders(headers);
            }
            return this;
        }

        public HttpChannel WithCookies(HttpCookies cookies, bool createNew)
        {
            if (createNew)
            {
                Cookies = cookies;
            }
            else
            {
                Cookies.AddCookies(cookies);
            }
            return this;
        }

        /// <summary>
        /// Authentication
        /// </summary>
        /// <param name="authentication"></param>
        /// <returns></returns>
        public HttpChannel WithAuthentication(HttpAuthentication authentication)
        {
            Authentication = authentication;
            return this;
        }

        /// <summary>
        /// Entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public HttpChannel WithEntity(HttpEntity entity)
        {
            Entity = entity;
            return this;
        }

    }
}