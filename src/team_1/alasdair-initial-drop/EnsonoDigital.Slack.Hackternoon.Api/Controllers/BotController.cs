using EnsonoDigital.Slack.Hackternoon.Api.RequestModels;
using EnsonoDigital.Slack.Hackternoon.Application;
using Microsoft.AspNetCore.Mvc;

namespace EnsonoDigital.Slack.Hackternoon.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BotController : ControllerBase
    {
        private readonly ILogger<BotController> _logger;
        private readonly IBotClient _botClient;

        public BotController(ILogger<BotController> logger, IBotClient botClient)
        {
            _logger = logger;
            _botClient = botClient;
        }

        [HttpPost]
        [Route("SendMessageToChat")]
        public async Task<IActionResult> SendMessageToChat(SendMessageToChatModel requestModel)
        {
            await _botClient.SendMessageAsync(requestModel.Channel, requestModel.Message);

            return Accepted();
        }

        [HttpPost]
        [Route("ListMemberInChannel")]
        public async Task<IActionResult> ListMemberInChannel(ListMembersInChannelModel requestModel)
        {
            await _botClient.ListMembersInChannel(requestModel.Channel);

            return Accepted();
        }
    }
}