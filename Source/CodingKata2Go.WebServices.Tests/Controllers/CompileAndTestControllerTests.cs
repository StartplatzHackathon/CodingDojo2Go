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
            var implPath = AssemblyDirectory + "\\..\\..\\SampleTest.cs";
            var testPath = AssemblyDirectory + "\\..\\..\\SampleTest.cs";

            var request = new KataRequest();
            request.ImplementationCode = File.ReadAllText(implPath);
            request.TestCode = File.ReadAllText(testPath);
            var controller = new CompileAndTestController();

            var result = controller.Post(request);
            Assert.Fail("fail");
        }

        [Test]
        public void TestSample_GetCompileError()
        {
            var request = new KataRequest();

            request.ImplementationCode = ReadResource("CompileError.cs");
            request.TestCode = ReadResource("SimpleTestClass.cs");

            var controller = new CompileAndTestController();

            var result = controller.Post(request);

            result.CompileErrors.ForEach(x => Console.WriteLine(x.Area));

            Assert.AreEqual(1, result.CompileErrors.Count);
        }

        private static string ReadResource(string resourceName)
        {
            using (Stream stream = Assembly.GetExecutingAssembly()
                                           .GetManifestResourceStream("CodingKata2Go.WebServices.Tests.CodeTestClasses." +
                                                                      resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}