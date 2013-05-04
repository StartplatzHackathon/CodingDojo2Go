using System.CodeDom.Compiler;
using System.Text;
using Microsoft.CSharp;

namespace CodingKata2Go.Infrastructure
{
    public class Compiler
    {
        public void Compile(string sourceCode, string outputAssembly)
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
                var errors = new StringBuilder();
                foreach (CompilerError error in results.Errors)
                {
                    if (!error.IsWarning)
                    {
                        errors.AppendLine("Compile Error: Line (" + error.Line + ") - " + error.ErrorText);
                    }
                }
                throw new CompileException(errors.ToString());
            }
        }
    }
}