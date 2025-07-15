namespace Wealth.CurrencyManagement.Application.Abstractions;

public class DefferedCommand
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

    private DefferedCommand()
    {
    }

    public static DefferedCommand Create(
        IJsonSerializer jsonSerializer,
        DateTimeOffset occurredOn,
        object obj)
    {
        return Create(jsonSerializer, occurredOn, obj, null);
    }

    public static DefferedCommand CreateDelayed(
        IJsonSerializer jsonSerializer,
        DateTimeOffset occurredOn,
        object obj,
        DateTimeOffset processingDate)
    {
        return Create(jsonSerializer, occurredOn, obj, processingDate);
    }

    private static DefferedCommand Create(
        IJsonSerializer jsonSerializer,
        DateTimeOffset occurredOn,
        object obj,
        DateTimeOffset? processingDate)
    {
        var message = new DefferedCommand();
        message.Id = Guid.NewGuid();
        message.OccurredOn = occurredOn;
        message.AssemblyName = obj.GetType().Assembly.GetName().Name!;
        message.Type = obj.GetType().FullName!;

        message.Data = jsonSerializer.Serialize(obj);
        message.ProcessingDate = processingDate;

        return message;
    }
}