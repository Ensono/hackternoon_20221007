using System;
using System.Dynamic;
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
        public record AttractionDataFile(AttractionData[] Data);

        public record AttractionData(string Name);

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

            string attractions;

            switch (request.Text)
            {
                case "london":
                    attractions = await File.ReadAllTextAsync("Offices/london.json");
                    break;
                case "cardiff":
                    attractions = await File.ReadAllTextAsync("Offices/cardiff.json");
                    break;
                case "manchester":
                    attractions = await File.ReadAllTextAsync("Offices/manchester.json");
                    break;
                default:
                    return new OkObjectResult("404 Office not found");
            }

            var parsedAttractions = JsonConvert.DeserializeObject<AttractionDataFile>(attractions).Data;

            var blocks = new List<object>
            {
                new
                {
                    Type = "header",
                    Text = new
                    {
                        Type = "plain_text",
                        Text = "Recommendations near " + request.Text
                    }
                }
            };

            blocks.AddRange(parsedAttractions
                .Select(pa => new
                {
                    Type = "section",
                    Text = new
                    {
                        Type = "plain_text",
                        Text = pa.Name
                    }
                }));

            return new OkObjectResult(new
            {
                blocks
            });
        }
    }
}
