namespace EMed.Infrastructure;

public class EfCoreSettings
{
    public bool EnableSensitiveDataLogging { get; set; }
    public bool EnableDetailedErrors { get; set; }
    public int CommandTimeout { get; set; }
}