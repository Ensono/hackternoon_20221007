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

            var numberOfPeopleInTeam = conversationMembers.members.Length;
            var numberOfPeopleOff = 0;

            var response = new List<string>();
            response.Add("Here is a summary of team members who are out of office:" + Environment.NewLine);
            var bobUsers = _bobClient.GetOutOfOfficeEmployees();
            var outOfOfficeEmailAddresses = bobUsers.Select(e => e.Email.ToLower());

            foreach (var conversationMember in conversationMembers.members)
            {
                var user = userListResponse.members.First(m => m.id == conversationMember);

                if (outOfOfficeEmailAddresses.Contains(user.profile.email.ToLower()))
                {
                    var bobUser = bobUsers.First(b => b.Email.ToLower() == user.profile.email.ToLower());

                    numberOfPeopleOff++;
                    response.Add($"Email: {bobUser.Email}, Competency: {bobUser.Competency}");
                }
            }

            int percentageOfTeamOff = 100 * numberOfPeopleOff / numberOfPeopleInTeam;

            response.Add(Environment.NewLine + $"{percentageOfTeamOff}% of the team is out of office.");

            var channelMembersMessage = string.Join(Environment.NewLine, response);

            await _slackTaskClient.PostMessageAsync(channel, channelMembersMessage);
        }
    }
}
