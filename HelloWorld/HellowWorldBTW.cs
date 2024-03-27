using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace BTW.HelloWorld
{
    public class HellowWorldBTW
    {
        private readonly ILogger<HellowWorldBTW> _logger;

        public HellowWorldBTW(ILogger<HellowWorldBTW> logger)
        {
            _logger = logger;
        }

        [Function("HellowWorldBTW")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult("Hellow Ben!!!!!");
        }
    }
}
