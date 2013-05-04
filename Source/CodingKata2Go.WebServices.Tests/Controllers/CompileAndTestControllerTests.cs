using System;
using System.IO;
using System.Reflection;
using CodingKata2Go.WebServices.Controllers;
using CodingKata2Go.WebServices.Models;
using NUnit.Framework;

namespace CodingKata2Go.WebServices.Tests.Controllers
{
    [TestFixture]
    public class CompileAndTestControllerTests
    {
        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                var uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        [Test]
        public void TestSampleTest()
        {
            string path = AssemblyDirectory + "\\..\\..\\SampleTest.cs";

            var controller = new CompileAndTestController();

            CompileAndTestResult result = controller.Post(File.ReadAllText(path));
            Assert.Fail("fail");
        }
    }
}