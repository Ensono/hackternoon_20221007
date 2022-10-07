using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SlackAPI;

namespace EnsonoDigital.Slack.Hackternoon.Application
{
    public static class ServiceCollectionExtensions
    {
        public static void AddHackternoonApplication(this IServiceCollection services, IConfiguration configuration)
        {
            var slackConfig = new SlackConfig();
            var slackConfigSection = configuration.GetSection(nameof(SlackConfig));
            slackConfigSection.Bind(slackConfig);
            
            var slackTaskClient = new SlackTaskClient(slackConfig.OAuthToken);

            services.AddSingleton(slackTaskClient);
            services.AddSingleton<IBotClient, BotClient>();
        }
    }
}