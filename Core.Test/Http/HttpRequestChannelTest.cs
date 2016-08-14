using System;
using System.Net;
using System.Text;
using Core.Collection;
using Core.Http;
using Core.Http.Entities;
using Core.Http.Enums;
using NUnit.Framework;

namespace Core.Test.Http {

    [TestFixture]
    public class HttpRequestChannelTest {

        [Test]
        public void TestUrl()
        {
            var url = HttpUrl.From("https://google.com.au/");
            Checks.Equals(new Uri("https://google.com.au"), url.Uri);

            var httpUrl = new HttpUrl()
                    .WithHost("google.com.au")
                    .WithSchema(HttpSchema.Http)
                    .WithHash("test")
                    .WithPath("/a/b/c")
                    .WithPort(123456)
                    .WithParameter("name",  "value")
                    .WithParameter("name2", 1)
                    .WithParameter("name2", 2);

            Console.WriteLine(httpUrl);

        }

        [Test]
        public void TestHost()
        {
            var url = HttpUrl.From("http://www.google.com.au");
            Checks.Equals(new Uri("http://www.google.com.au/"), url.Uri);
        }

        [Test]
        public void TestGet()
        {
            var response = HttpChannel.NewChannel(HttpUrl.From("http://www.expressone.com.au/zh-cn/"))
                .WithAccept("text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8")
                .WithUserAgent("Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.111 Safari/537.36")
                .WithHeader(HttpRequestHeader.AcceptLanguage, "en-US,en;q=0.8")
                .WithHeader(HttpRequestHeader.AcceptEncoding, "gzip, deflate, sdch")
                .WithHeader("x-hd-token", "hello")
                .WithHeader("Upgrade-Insecure-Requests", 1)
                .Get();

            Console.WriteLine(response.StatusCode);
            Console.WriteLine(response.Error?.Message);

            Console.WriteLine("Headers ------------------------");
            response.Headers.AsReadOnly().ForEach(pair =>
            {
                Console.WriteLine($"{pair.Key}|{pair.Value}");
            });

            Console.WriteLine("Cookies ------------------------");
            response.Cookies.AsReadOnly().ForEach(pair => {
                Console.WriteLine($"{pair.Key}|{pair.Value}");
            });

            Checks.Equals(true, response.Success);
        }

        [Test]
        public void TestPost()
        {
            var entity = HttpEntity.UrlEncoded()
                                   .AddParameter("ordername", "123233")
                                   .AddParameter("rs", "sear")
                                   ;

            Console.WriteLine(entity.ContentType);
            Console.WriteLine(entity.ContentLength);
            Console.WriteLine(Encoding.UTF8.GetString(entity.GetBody()));

            var response = HttpChannel.NewChannel("http://www.expressone.com.au/zh-cn//do/face.php")
                                      .WithAccept("text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8")
                                      .WithUserAgent("Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.111 Safari/537.36")
                                      .WithCookie("PHPSESSI", "ed85be2b2c0d283b20dfaf95ec3d5e03")
                                      .Send(entity);

            Console.WriteLine(response.StatusCode);
            Console.WriteLine(response.GetTextContent());

        }

        [Test]
        public void TestForm()
        {

        }

    }
}