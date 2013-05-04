using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodingKata2Go.WebServices.Controllers;
using NUnit.Framework;

namespace CodingKata2Go.WebServices.Tests.Controllers
{
    [TestFixture]
    public class CompileAndTestControllerTests
    {
        public void TestSampleTest()
        {
            var path = @"C:\Users\wickelr\Documents\GitHub\CodingDojo2Go\Source\CodingKata2Go.WebServices.Tests\SampleTest.cs";

            var controller = new CompileAndTestController();

            var result = controller.Post(File.ReadAllText(path));
        }
    }
}
