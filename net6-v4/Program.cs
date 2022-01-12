using Microsoft.Extensions.Hosting;

namespace Poc
{
    public static class Program
    {
        public static void Main()
        {
            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults(workerApplication =>
                {
                    workerApplication.UseMiddleware<AuthorizationMiddleware>();
                })
                .Build();

            host.Run();
        }
    }
}