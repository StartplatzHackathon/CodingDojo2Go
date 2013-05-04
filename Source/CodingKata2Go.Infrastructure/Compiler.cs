using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Text;
using CodingKata2Go.Infrastructure.Model;
using Microsoft.CSharp;

namespace CodingKata2Go.Infrastructure
{
    public class Compiler
    {
        public List<CompileError> Compile(string sourceCode, string outputAssembly)
        {
            var parms = new CompilerParameters();
            parms.GenerateExecutable = false;
            parms.GenerateInMemory = false;
            parms.OutputAssembly = outputAssembly;
            parms.IncludeDebugInformation = false;
            parms.ReferencedAssemblies.Add("System.dll");
            parms.ReferencedAssemblies.Add("System.Core.dll");
            parms.ReferencedAssemblies.Add("nunit.framework.dll");

            var provider = new CSharpCodeProvider();
            CompilerResults results = provider.CompileAssemblyFromSource(parms, sourceCode);

            if (results.Errors.HasErrors)
            {
                var errors = new List<CompileError>();
                foreach (CompilerError error in results.Errors)
                {
                    errors.Add(new CompileError { Column = error.Column, ErrorNumber = error.ErrorNumber, ErrorText = error.ErrorText, IsWarning = error.IsWarning, Line = error.Line });
                }
                return errors;
            }
            return null;
        }
    }
}