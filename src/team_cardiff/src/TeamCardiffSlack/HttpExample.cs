using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace TeamCardiffSlack
{
    public static class HttpExample
    {
        [FunctionName("HttpExample")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] SlackRequest request,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            log.LogInformation($"Text: {request.Text}");

            switch (request.Text)
            {
                case "London":
                    return new OkObjectResult(await File.ReadAllTextAsync("Offices/london.json"));
                case "Cardiff":
                    return new OkObjectResult(await File.ReadAllTextAsync("Offices/cardiff.json"));
                case "Manchester":
                    return new OkObjectResult(await File.ReadAllTextAsync("Offices/manchester.json"));
                default:
                    return new OkObjectResult("404 Office not found");
            }
        }
    }
}
