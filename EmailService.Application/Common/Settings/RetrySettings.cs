public class RetrySettings
{
    public int MaxRetryCount { get; set; } = 3;
    public int DelaySeconds { get; set; } = 2;
    public int BatchSize { get; set; } = 20;
    public int RetryIntervalMinutes { get; set; } = 5;
}