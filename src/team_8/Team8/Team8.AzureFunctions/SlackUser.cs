using Newtonsoft.Json;

namespace Team8.AzureFunctions;

public record SlackUser(
    string Id,
    [property:JsonProperty("real_name")] string RealName,
    SlackUserProfile Profile);