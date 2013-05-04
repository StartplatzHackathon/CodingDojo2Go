using System;

namespace CodingKata2Go.Infrastructure
{
    public class CompileException : Exception
    {
        public CompileException(string messae) : base(messae)
        {
        }
    }
}