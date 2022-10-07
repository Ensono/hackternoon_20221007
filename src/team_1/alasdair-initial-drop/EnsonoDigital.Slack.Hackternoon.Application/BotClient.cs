using SlackAPI;

namespace EnsonoDigital.Slack.Hackternoon.Application
{
    public class BotClient : IBotClient
    {
        private readonly SlackTaskClient _slackTaskClient;

        public BotClient(SlackTaskClient slackTaskClient)
        {
            _slackTaskClient = slackTaskClient;
        }

        public async Task SendMessageAsync(string channel, string message)
        {
            await _slackTaskClient.PostMessageAsync(channel, message);
        }
    }
}
