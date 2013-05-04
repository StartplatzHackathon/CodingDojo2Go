using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using CodingKata2Go.Infrastructure.Model;
using Microsoft.CSharp;

namespace CodingKata2Go.Infrastructure
{
    public class Compiler
    {
        public List<CompileError> Compile(string implementationCode, string testCode, string outputAssembly)
        {
            string implCodeFileName = Path.GetTempFileName();
            string testCodeFileName = Path.GetTempFileName();
            File.WriteAllText(implCodeFileName, implementationCode);
            File.WriteAllText(testCodeFileName, testCode);

            var parms = new CompilerParameters();
            parms.GenerateExecutable = false;
            parms.GenerateInMemory = true;
            parms.OutputAssembly = outputAssembly;
            parms.IncludeDebugInformation = false;
            parms.ReferencedAssemblies.Add("System.dll");
            parms.ReferencedAssemblies.Add("System.Core.dll");
            parms.ReferencedAssemblies.Add("nunit.framework.dll");

            var provider = new CSharpCodeProvider();
            CompilerResults results = provider.CompileAssemblyFromFile(parms, new[] { implCodeFileName, testCodeFileName });

            if (results.Errors.HasErrors)
            {
                var errors = new List<CompileError>();
                foreach (CompilerError error in results.Errors)
                {
                    errors.Add(new CompileError { Column = error.Column, ErrorNumber = error.ErrorNumber, Area = error.FileName == implementationCode ? CodeArea.Implementation : CodeArea.Test, ErrorText = error.ErrorText, IsWarning = error.IsWarning, Line = error.Line });
                }
                return errors;
            }
            return null;
        }
    }

}