using System;
using System.Collections.Generic;
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

        public record AttractionData(string Name, int Distance, [param:JsonProperty("address_obj")]AddressObj Address);
        
        public record AddressObj(string Street1, string Street2, string City, string Postalcode, string Phone);

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

            switch (text.ToLowerInvariant())
            {
                case "london":
                    attractions = Offices.London;
                    break;
                case "cardiff":
                    attractions = Offices.Cardiff;
                    break;
                case "manchester":
                    attractions = Offices.Manchester;
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
                        Text = "Recommendations near " + text
                    }
                }
            };
            
            blocks.AddRange(parsedAttractions
                .SelectMany(pa =>
                {

                    var addressStr = new List<string>
                    {
                        pa.Address.Street1,
                        pa.Address.Street2,
                        pa.Address.City,
                        pa.Address.Postalcode
                    };

                    var add = addressStr.Where(x => !string.IsNullOrEmpty(x)).ToArray();

                    return new object[]
                    {
                        new
                        {
                            Type = "header",
                            Text = new
                            {
                                Type = "plain_text",
                                Text = pa.Name
                            }
                        },
                        new
                        {
                            Type = "section",
                            Text = new
                            {
                                Type = "mrkdwn",
                                Text = $"*Address:*\n{string.Join("\n",add)}"
                            },
                            Accessory = new
                            {
                                Type = "button",
                                Text = new
                                {
                                    Type = "plain_text",
                                    Text = "Directions"
                                },
                                Value = "Click",
                                Url = $"https://www.google.co.uk/maps/place/{string.Join("+",add.Select(i => i.Replace(" ", "+")))}",
                                Action_id = "button-action"
                            }
                        },
                        new
                        {
                            Type = "section",
                            Text = new
                            {
                                Type = "mrkdwn",
                                Text = $"*Phone:* {pa.Address.Phone}"
                            }
                        },
                        new
                        {
                            Type = "section",
                            Text = new
                            {
                                Type = "mrkdwn",
                                Text = $"*Distance:* {pa.Distance} mile"
                            }
                        },
                    };
                }));

            return new OkObjectResult(new
            {
                Response_type="in_channel",
                blocks
            });
        }
    }
}