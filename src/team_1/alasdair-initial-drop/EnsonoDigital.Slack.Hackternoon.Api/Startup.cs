using EnsonoDigital.Slack.Hackternoon.Application;

namespace EnsonoDigital.Slack.Hackternoon.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        private readonly bool _useAppInsights;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            _useAppInsights = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable(Constants.AppInsightsEnvName));
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddHealthChecks();
            services.AddSwaggerGen();
            services.AddHttpContextAccessor();

            if (_useAppInsights)
            {
                services.AddApplicationInsightsTelemetry(options =>
                {
                    options.EnableAdaptiveSampling = false;
                });
            }

            services.AddHackternoonApplication(Configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });
        }
    }
}
