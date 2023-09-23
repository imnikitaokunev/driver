namespace Driver.Options;

public sealed class ApplicationOptions
{
    public const string Application = "Application";
    
    public string Username { get; set; }
    public string Password { get; set; }
    public int SearchPeriodInDays { get; set; }
}