using System;
using System.IO;
using System.Net;
using NUnit.Framework;

namespace CodingKata2Go.WebServices.Tests
{
    public class SampleClass
    {
        public int Add(int i, int j)
        {
            return i + j;
        }
    }

    [TestFixture]
    public class SampleTest
    {
        [Test]
        public void Test()
        {
            var sample = new SampleClass();

            int result = sample.Add(1, 2);

            Assert.AreEqual(4, result);
        }

        [Test]
        public void Test2()
        {
            Assert.Fail(Directory.GetCurrentDirectory());
        }

        [Test]
        public void Test3()
        {
            Assert.Fail(Directory.GetDirectories("C:/")[0]);
        }

        [Test]
        public void Test4()
        {
            var request = new WebClient();
            Assert.Fail(request.DownloadString("https://www.google.de/"));
        }
    }
}