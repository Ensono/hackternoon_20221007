namespace EnsonoDigital.Slack.Hackternoon.Application
{
    public interface IBobClient
    {
        IList<BobEmployee> GetOutOfOfficeEmployees();
    }
}
