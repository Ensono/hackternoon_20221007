using Newtonsoft.Json;

namespace Team8.AzureFunctions;

public record SlackUserProfile(
    [property:JsonProperty("status_text")] string StatusText,
    [property:JsonProperty("status_emoji")] string StatusEmoji);