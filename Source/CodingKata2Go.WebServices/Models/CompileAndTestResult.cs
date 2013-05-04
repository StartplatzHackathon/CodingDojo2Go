using System.Collections.Generic;
using CodingKata2Go.Infrastructure.Model;

namespace CodingKata2Go.WebServices.Models
{
    public class CompileAndTestResult
    {
        public List<CompileError> CompileErrors{ get; set; }
        public List<TestError>  TestErrors { get; set; }
    }
}