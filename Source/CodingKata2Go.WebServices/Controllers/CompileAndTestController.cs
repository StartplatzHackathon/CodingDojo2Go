using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CodingKata2Go.Infrastructure;
using CodingKata2Go.Infrastructure.Sandboxing;

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
        public string Post([FromBody]string value)
        {
            Sandbox sandbox = null;
            try
            {
                var sourceCode = value;

                var kataAssemblyPath = Path.GetTempFileName();

                var compiler = new Compiler();
                compiler.Compile(sourceCode, kataAssemblyPath);

                sandbox = Sandbox.Create();

                var result = sandbox.RunNunitTest(kataAssemblyPath);

                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
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
