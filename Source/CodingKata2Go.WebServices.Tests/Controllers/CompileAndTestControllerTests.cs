using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CodingKata2Go.WebServices.Controllers;
using NUnit.Framework;

namespace CodingKata2Go.WebServices.Tests.Controllers
{
    [TestFixture]
    public class CompileAndTestControllerTests
    {
        static public string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        [Test]
        public void TestSampleTest()
        {
            var path = AssemblyDirectory + "\\..\\..\\SampleTest.cs";

            var controller = new CompileAndTestController();

            var result = controller.Post(File.ReadAllText(path));
           Assert.Fail("fail");

        }
    }
}
