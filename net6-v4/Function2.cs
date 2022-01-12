using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace Poc
{
    public class Function2
    {
        [Function("Function2")]
        public async Task<HttpResponseData> RunAsync(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "test2")] HttpRequestData req
        )
        {
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteStringAsync("Test");
            return response;
        }
    }
}