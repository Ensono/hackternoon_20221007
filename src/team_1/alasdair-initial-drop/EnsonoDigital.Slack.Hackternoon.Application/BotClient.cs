using SlackAPI;

namespace EnsonoDigital.Slack.Hackternoon.Application
{
    public class BotClient : IBotClient
    {
        private readonly SlackTaskClient _slackTaskClient;
        private readonly IBobClient _bobClient;

        public BotClient(SlackTaskClient slackTaskClient, IBobClient bobClient)
        {
            _slackTaskClient = slackTaskClient;
            _bobClient = bobClient;
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

        public async Task ListMembersOutOfOfficeCommand(string channel)
        {
            var userListResponse = await _slackTaskClient.GetUserListAsync();

            var conversationsListResponse = await _slackTaskClient.GetConversationsListAsync(limit: 10000);
            var conversation = conversationsListResponse.channels.First(c => c.name == channel);
            var conversationMembers = await _slackTaskClient.GetConversationsMembersAsync(conversation.id);

            var response = new List<string>();
            var outOfOfficeEmailAddresses = _bobClient.GetOutOfOfficeEmployees().Select(e => e.Email.ToLower());

            foreach (var conversationMember in conversationMembers.members)
            {
                var user = userListResponse.members.First(m => m.id == conversationMember);

                if (outOfOfficeEmailAddresses.Contains(user.profile.email.ToLower()))
                {
                    response.Add(user.profile.email);
                }
            }

            var channelMembersMessage = string.Join(Environment.NewLine, response);

            await _slackTaskClient.PostMessageAsync(channel, channelMembersMessage);
        }
    }
}
