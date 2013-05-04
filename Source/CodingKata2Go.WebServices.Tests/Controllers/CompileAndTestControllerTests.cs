using System;
using System.Linq;
using System.IO;
using System.Reflection;
using CodingKata2Go.Infrastructure.Model;
using CodingKata2Go.WebServices.Controllers;
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

            var request = new KataRequest
                {
                    ImplementationCode = File.ReadAllText(implPath),
                    TestCode = File.ReadAllText(testPath)
                };
            var controller = new CompileAndTestController();

            var result = controller.Post(request);
            Assert.Fail("fail");
        }

        [Test]
        public void TestSample_GetCompileErrorInImplementation()
        {
            var request = new KataRequest
                {
                    ImplementationCode = ReadResource("CompileError.cs"),
                    TestCode = ReadResource("SimpleTestClass.cs")
                };

            var controller = new CompileAndTestController();

            var result = controller.Post(request);

            var compileErrors = result.CompileErrors.ToList();
            compileErrors.ForEach(x => Console.WriteLine(x.Area));

            Assert.IsNotNull(result.CompileErrors);
            Assert.AreEqual(1, compileErrors.Count);
            Assert.AreEqual(CodeArea.Implementation, compileErrors.First().Area);
        }

        private static string ReadResource(string resourceName)
        {
            using (var stream = Assembly.GetExecutingAssembly()
                                           .GetManifestResourceStream("CodingKata2Go.WebServices.Tests.CodeTestClasses." +
                                                                      resourceName))
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}