using System;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
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
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string requestBody = String.Empty;
            using (StreamReader streamReader =  new  StreamReader(req.Body))
            {
                requestBody = await streamReader.ReadToEndAsync();
            }

            var data = requestBody.Split("&");

            var textParam = data.FirstOrDefault(x => x.StartsWith("text="));

            var text = textParam.Replace("text=", "").Trim();
            
            
            log.LogInformation("C# HTTP trigger function processed a request.");
            log.LogInformation($"Text: {text}");

            switch (text)
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
