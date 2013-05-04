using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CodingKata2Go.Infrastructure.Model;
using Microsoft.CSharp;
using NUnit.Framework;

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

            try
            {
                var parms = new CompilerParameters
                    {
                        GenerateExecutable = false,
                        GenerateInMemory = true,
                        OutputAssembly = outputAssembly,
                        IncludeDebugInformation = false
                    };
                parms.ReferencedAssemblies.Add("System.dll");
                parms.ReferencedAssemblies.Add("System.Core.dll");
                parms.ReferencedAssemblies.Add(typeof(TestAttribute).Assembly.Location);

                var provider = new CSharpCodeProvider();
                CompilerResults results = provider.CompileAssemblyFromFile(parms, new[]
                    {
                        implCodeFileName, testCodeFileName
                    });

                if (results.Errors.HasErrors)
                {
                    return (from CompilerError error in results.Errors
                            select new CompileError
                                {
                                    Column = error.Column,
                                    ErrorNumber = error.ErrorNumber,
                                    Area = error.FileName == implementationCode ? CodeArea.Implementation : CodeArea.Test,
                                    ErrorText = error.ErrorText,
                                    IsWarning = error.IsWarning,
                                    Line = error.Line
                                }).ToList();
                }
                return null;
            }
            finally
            {
                DeleteIfExists(implCodeFileName);
                DeleteIfExists(testCodeFileName);
            }
        }

        private void DeleteIfExists(string filename)
        {
            if (File.Exists(filename))
            {
                File.Delete(filename);
            }
        }
    }
}