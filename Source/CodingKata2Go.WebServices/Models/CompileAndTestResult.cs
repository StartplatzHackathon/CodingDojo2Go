using System.Collections.Generic;

namespace CodingKata2Go.WebServices.Models
{
    public class CompileAndTestResult
    {
        public IEnumerable<CompileError> CompileErrors { get; set; }
        public IEnumerable<TestError> TestErrors { get; set; }
    }
}