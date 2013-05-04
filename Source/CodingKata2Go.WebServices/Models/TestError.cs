using System;

namespace CodingKata2Go.WebServices.Models
{
    public class TestError
    {
        public string TestMethod { get; set; }
        public string TestClass { get; set; }
        public string ErrorMessage { get; set; }
        public string StackTrace { get; set; }
    }
}