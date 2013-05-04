using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using CodingKata2Go.Infrastructure;
using CodingKata2Go.Sandbox;
using CodingKata2Go.WebServices.Models;

namespace CodingKata2Go.WebServices.Controllers
{
    public class CompileAndTestController : ApiController
    {
        //// GET api/compileandtest
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/compileandtest/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST api/compileandtest //[FromBody]
        public CompileAndTestResult Post(KataRequest request)
        {
            Sandboxer sandbox = null;
            string kataAssemblyPath = Path.GetTempFileName();
            try
            {
                var compiler = new Compiler();
                var compileResult = compiler.Compile(request.ImplementationCode ?? "", request.TestCode ?? "", kataAssemblyPath).Select(ToContract).ToList();
                var result = new CompileAndTestResult
                    {
                        CompileErrors = compileResult
                    };
                
                if (compileResult.Any(x => !x.IsWarning))
                    return result;

                sandbox = Sandboxer.Create();
                List<Sandbox.Model.TestError> testResult = sandbox.RunNunitTest(kataAssemblyPath);
                result.TestErrors = testResult.Select(ToContract);

                return result;
            }
            finally
            {
                if (sandbox != null)
                    AppDomain.Unload(sandbox.AppDomain);

                DeleteIfExists(kataAssemblyPath);
            }
        }

        //// PUT api/compileandtest/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/compileandtest/5
        //public void Delete(int id)
        //{
        //}

        private static TestError ToContract(Sandbox.Model.TestError sandboxTestError)
        {
            if (sandboxTestError == null)
            {
                return null;
            }

            return new TestError
            {
                TestClass = sandboxTestError.TestClass,
                TestMethod = sandboxTestError.TestMethod,
                StackTrace = sandboxTestError.StackTrace,
                ErrorMessage = sandboxTestError.ErrorMessage,
            };
        }

        private static CompileError ToContract(Infrastructure.Model.CompileError sandboxCompileError)
        {
            if (sandboxCompileError == null)
            {
                return null;
            }

            return new CompileError
            {
                Column = sandboxCompileError.Column,
                Line = sandboxCompileError.Line,
                ErrorNumber = sandboxCompileError.ErrorNumber,
                ErrorText = sandboxCompileError.ErrorText,
                IsWarning = sandboxCompileError.IsWarning,
            };
        }

        private void DeleteIfExists(string filename)
        {
            try
            {
                if (File.Exists(filename))
                {
                    File.Delete(filename);
                }
            }
            catch (Exception)
            {
                // TODO
            }
        }
    }
}