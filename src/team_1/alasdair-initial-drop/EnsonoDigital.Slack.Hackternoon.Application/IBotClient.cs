namespace EnsonoDigital.Slack.Hackternoon.Application
{
    public interface IBotClient
    {
        Task SendMessageAsync(string channel, string message);
    }
}
