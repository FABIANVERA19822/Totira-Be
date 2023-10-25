using System;
namespace Totira.Services.Thirdparty.Worker.Outbox.Options;

public record SQSOptions
{
    public static string SectionName = "SQSSettings";

    public string Region { get; set; } = string.Empty;
    public string AccountId { get; set; } = string.Empty;
    public string QueueName { get; set; } = string.Empty;
    public string AccessKey { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
    public string ServiceUrl { get; set; } = string.Empty;
    public bool UseLocalStack { get; set; }
    public int GetMessagesIntervalInSeconds { get; set; } = 2;
    public int AmountOfMessagesPerInterval { get; set; } = 5;
}

