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
            await _slackTaskClient.PostMessageAsync($"#{channel}", message);
        }

        public async Task ListMembersInChannel(string channel)
        {
            var userListResponse = await _slackTaskClient.GetUserListAsync();

            var conversationsListResponse = await _slackTaskClient.GetConversationsListAsync(limit: 10000);
            var conversation = conversationsListResponse.channels.First(c => c.name == channel);
            var conversationMembers = await _slackTaskClient.GetConversationsMembersAsync(conversation.id);

            var channelMembers = new List<string>();

            foreach (var conversationMember in conversationMembers.members)
            {
                var user = userListResponse.members.First(m => m.id == conversationMember);

                channelMembers.Add($"{user.name} <{user.profile.email}>");
            }

            var channelMembersMessage = string.Join(Environment.NewLine, channelMembers);

            await _slackTaskClient.PostMessageAsync(channel, channelMembersMessage);
        }
    }
}
