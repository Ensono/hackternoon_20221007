namespace Team8.AzureFunctions;

public record EventBody(string Type, SlackUser? User );