using System.Collections.Generic;

namespace CodingKata2Go.WebServiceReferences
{
    public class CompileAndTestResult
    {
        public IEnumerable<CompileError> CompileErrors { get; set; }
        public IEnumerable<TestError> TestErrors { get; set; }
    }
}