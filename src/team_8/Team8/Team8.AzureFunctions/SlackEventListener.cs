using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Team8.AzureFunctions;

public static class SlackEventListener
{
    [FunctionName("SlackEventListener")]
    public static async Task<IActionResult> RunAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] SlackEvent req, ILogger log)
    {
        if (req.Challenge != null)
        {
            log.LogInformation("Returning challenge response");
            return new OkObjectResult(new ChallengeResponse(req.Challenge));
        }

        if (req.Event?.Type == SlackStringConstants.UserStatusChanged 
            && req.Event.User?.Profile.StatusText == SlackStringConstants.OffSickStatusText)
        {
            log.LogInformation("User status changed received for {UserId}: {Name}", req.Event.User.Id, req.Event.User.RealName);
            var emailDispatcher = new EmailDispatcher();
            emailDispatcher.Send(req.Event.User.RealName);
            return new OkObjectResult(new ChangeResponse(req.Event.User.Id, req.Event.User.RealName));
        }

        return new NoContentResult();
    }
}