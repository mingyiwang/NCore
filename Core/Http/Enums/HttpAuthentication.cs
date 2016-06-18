using System;
using System.Text;

namespace Core.Http.Enums
{
    public sealed class HttpAuthentication
    {
        public enum HttpAuthenticaionKind
        {
            Network,
            Form
        }

        public HttpAuthenticaionKind Kind { get; set; }
        public string Name     { get; private set; }
        public string Password { get; private set; }

        private HttpAuthentication(string name, string password, HttpAuthenticaionKind kind)
        {
            Name = name;
            Password = password;
            Kind = kind;
        }

        /// <summary>
        /// This is normal form authentication
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static HttpAuthentication FormAuthentication(string name, string password)
        {
            return new HttpAuthentication(name, password, HttpAuthenticaionKind.Form);
        }

        /// <summary>
        /// This is the authentication we normally see in IE with Pop up
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static HttpAuthentication NetworkAuthentication(string name, string password)
        {
            return new HttpAuthentication(name, password, HttpAuthenticaionKind.Network);
        }

        public override string ToString()
        {
            return "BASIC " + Convert.ToBase64String(Encoding.UTF8.GetBytes($"{Name}:{Password}"));
        }


    }
}