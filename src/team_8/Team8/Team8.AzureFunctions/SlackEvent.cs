namespace Team8.AzureFunctions;

public class SlackEvent
{
    public string? Challenge { get; set; }

    public EventBody? Event { get; set; }
    
}