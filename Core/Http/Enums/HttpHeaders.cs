using System;
using System.Collections.Generic;
using System.Net;
using Core.Collection;
using static Core.Collection.Collections;

namespace Core.Http.Enums
{
    public class HttpHeaders : IEquatable<HttpHeaders>
    {
        private static readonly HashSet<string> IllegalHeaders = new HashSet<string> {
            "Accept",
            "Connection",
            "ContentType",
            "ContentLength",
            "Host",
            "Referer",
            "UserAgent"
        };

        private readonly HashMap<string, string> _headers = new HashMap<string, string>();

        public bool IsEmpty => _headers.Size() == 0;

        public IReadOnlyDictionary<string, string> AsReadOnly()
        {
            return Collections.AsReadOnly(_headers);
        }
         
        public HttpHeaders AddHeader(HttpRequestHeader header, string value)
        {
            var name = Enum.GetName(header.GetType(), header);
            AddHeader(name, value);
            return this;
        }

        public HttpHeaders AddHeader(string name, string value)
        {
            PreConditions.CheckFalse(IllegalHeaders.Contains(name), $"{name} must not set here.");
            _headers.Put(name, value);
            return this;
        }

        public HttpHeaders AddHeaders(HttpHeaders headers)
        {
            if (!headers.IsEmpty)
            {
                foreach(var pair in headers.AsReadOnly())
                {
                    AddHeader(pair.Key, pair.Value);
                }
            }
            return this;
        }

        public HttpHeaders AddHeaders(WebHeaderCollection collection)
        {
            if (collection.Count > 0)
            {
                foreach(var headerName in collection.AllKeys)
                {
                    AddHeader(headerName, collection[headerName]);
                }
            }
            return this;
        }

        public bool Equals(HttpHeaders other)
        {
            return Equals(this, other);
        }

    }
}