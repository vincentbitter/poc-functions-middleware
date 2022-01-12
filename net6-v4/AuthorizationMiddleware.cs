using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;

namespace Poc
{
    public class AuthorizationMiddleware : IFunctionsWorkerMiddleware
    {
        public Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
        {
            return NotAuthenticated(context);
        }

        private static async Task NotAuthenticated(FunctionContext context)
        {
            var request = context.GetHttpRequestData();

            var response = request.CreateResponse(HttpStatusCode.Unauthorized);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            await response.WriteStringAsync("Not authenticated");

            context.SendHttpResponse(response);
        }
    }
}