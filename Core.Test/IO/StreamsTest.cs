using System;
using System.IO;
using System.Text;
using Core.IO;
using NUnit.Framework;

namespace Core.Test.IO
{
    [TestFixture]
    public class StreamsTest
    {
        const string Content = "dfdfdfdfdfdfdfdfdfdfdfdfdfdfdfdfdfdfdf";
        const string Path = "/";

        [Test]
        public void TestRead()
        {
            var stream = Streams.MemoryStream(Encoding.UTF8.GetBytes(Content));
            Check.Equals(Streams.GetString(stream), Content);
        }

        [Test]
        public void TestWrite()
        {
            var stream = Streams.MemoryStream(Encoding.UTF8.GetBytes(Content));
            Check.Equals(Streams.GetString(stream), Content);
            Streams.PutBytes(Encoding.UTF8.GetBytes(Content), null, Encoding.UTF8);
        }

        [Test]
        public void TestTransfer()
        {
            var input  = Streams.MemoryStream(Encoding.UTF8.GetBytes(Content));
            var output = new MemoryStream();

            Streams.Transfer(input, output, Encoding.UTF8);
            Check.Equals(Streams.GetString(output), Content);
        }

    }
}