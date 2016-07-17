using System;
using System.IO;
using System.Linq;
using Core.Collection;
using Core.Concurrent;
using Core.IO;
using Core.Net;
using NUnit.Framework;

namespace Core.Test.Net
{
    [TestFixture]
    public class MimeTypesTest
    {
        const string ResxTemplate = 
            @"<data name =""{0}"" xml:space=""preserve""><value>{1}</value></data>";

        [Test]
        public void TestGetString()
        {
            Console.WriteLine(MimeTypes.Instance.Of("atom"));
        }

        [Test]
        public void TestAccess()
        {
            var hashCode = MimeTypes.Instance.GetHashCode();
            Enumerable.Range(0, 100).ForEach(count =>
            {
                Threads.StartNew("Thread" + count, () =>
                {
                    Checking.CheckEquals(hashCode, MimeTypes.Instance.GetHashCode());
                });
            });
        }

        [Test]
        public void GenerateMimeTypes()
        {
            var mydocpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var content = Resources.GetString("mime.types", GetType());
            var lines   = content.Split(new[] {'\n'});
            using (var outputFile = new StreamWriter(mydocpath + @"\mimeTypes.txt", true))
            {
                lines.ForEach(line =>
                {
                    var tokens = line.Split('=');
                    var mimeType = tokens[0];
                    var token2 = tokens[1];
                    var extensions = token2.Split('|');
                    foreach (var e in extensions)
                    {
                        outputFile.WriteLine(ResxTemplate, e, mimeType);

                    }
                });

            }
        }
    }

}