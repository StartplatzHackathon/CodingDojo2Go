using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace CodingKata2Go.WebServiceReferences
{
    public class ServiceClient
    {
        private const string ServerUrl = "http://localhost:49293/";

        public async Task<CompileAndTestResult> CompileAndTestAsync(KataRequest kataRequest)
        {
            var client = new HttpClient
                {
                    BaseAddress = new Uri(ServerUrl)
                };

            HttpResponseMessage response = await client.PostAsJsonAsync("api/CompileAndTest", kataRequest);

            response.EnsureSuccessStatusCode();
            CompileAndTestResult result = await response.Content.ReadAsAsync<CompileAndTestResult>();

            return result;
        }
    }
}