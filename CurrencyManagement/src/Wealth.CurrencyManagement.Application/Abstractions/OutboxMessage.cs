namespace Wealth.CurrencyManagement.Application.Abstractions;

public class OutboxMessage
{
    public Guid Id { get; set; }
    public DateTimeOffset OccurredOn { get; set; }
    public string AssemblyName { get; set; }
    public string Type { get; set; }
    public string Data { get; set; }
    public DateTimeOffset? ProcessedDate { get; set; }
    public DateTimeOffset? ProcessingDate { get; set; }
    public string? Error { get; set; }

    public bool IsInstantProcessing => ProcessingDate is null;

    private OutboxMessage()
    {
    }

    public static OutboxMessage Create(
        IJsonSerializer jsonSerializer,
        DateTimeOffset occurredOn,
        object obj)
    {
        return Create(jsonSerializer, occurredOn, obj, null);
    }

    public static OutboxMessage CreateDelayed(
        IJsonSerializer jsonSerializer,
        DateTimeOffset occurredOn,
        object obj,
        DateTimeOffset processingDate)
    {
        return Create(jsonSerializer, occurredOn, obj, processingDate);
    }

    private static OutboxMessage Create(
        IJsonSerializer jsonSerializer,
        DateTimeOffset occurredOn,
        object obj,
        DateTimeOffset? processingDate)
    {
        var message = new OutboxMessage();
        message.Id = Guid.NewGuid();
        message.OccurredOn = occurredOn;
        message.AssemblyName = obj.GetType().Assembly.GetName().Name!;
        message.Type = obj.GetType().FullName!;

        message.Data = jsonSerializer.Serialize(obj);
        message.ProcessingDate = processingDate;

        return message;
    }
}