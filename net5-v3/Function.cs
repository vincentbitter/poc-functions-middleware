using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace Poc
{
    public class Function
    {
        [Function("Function")]
        public async Task<Output> RunAsync(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "test")] HttpRequestData req
        )
        {
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteStringAsync("Test");
            return new Output { Response = response };
        }
    }
}