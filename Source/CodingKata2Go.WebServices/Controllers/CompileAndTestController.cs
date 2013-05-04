using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CodingKata2Go.Infrastructure;
using CodingKata2Go.Infrastructure.Model;
using CodingKata2Go.Infrastructure.Sandboxing;
using CodingKata2Go.WebServices.Models;
using Elmah;

namespace CodingKata2Go.WebServices.Controllers
{
    public class CompileAndTestController : ApiController
    {
        // GET api/compileandtest
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/compileandtest/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/compileandtest
        public CompileAndTestResult Post([FromBody]string value)
        {
            Sandbox sandbox = null;
            try
            {
                var sourceCode = value;

                var kataAssemblyPath = Path.GetTempFileName();

                var compiler = new Compiler();
                var compileResult = compiler.Compile(sourceCode, kataAssemblyPath);
                var result = new CompileAndTestResult { CompileErrors = compileResult };
                if (compileResult != null && compileResult.Any(x => !x.IsWarning))
                    return result;

                sandbox = Sandbox.Create();
                var testResult = sandbox.RunNunitTest(kataAssemblyPath);
                result.TestErrors = testResult;

                return result;
            }
            catch (Exception ex)
            {
                //log.ErrorFormat("Error appeared on system.", ex); 
                ErrorSignal.FromCurrentContext().Raise(ex);
                throw new HttpResponseException(HttpStatusCode.InternalServerError);

                //TODO later format and catch nicely
            }
            finally
            {
                if (sandbox != null)
                    AppDomain.Unload(sandbox.AppDomain);
            }
        }

        // PUT api/compileandtest/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/compileandtest/5
        public void Delete(int id)
        {
        }
    }
}