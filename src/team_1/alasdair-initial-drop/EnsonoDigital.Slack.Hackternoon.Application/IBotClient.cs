namespace EnsonoDigital.Slack.Hackternoon.Application
{
    public interface IBotClient
    {
        Task SendMessageAsync(string channel, string message);

        Task ListMembersInChannel(string channel);

        Task ListMembersOutOfOfficeCommand(string channel);
    }
}
