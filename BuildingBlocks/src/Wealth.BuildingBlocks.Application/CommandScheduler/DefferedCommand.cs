using System.Text.Json;

namespace Wealth.BuildingBlocks.Application.CommandScheduler;

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
        DateTimeOffset occurredOn,
        ICommand command)
    {
        return Create(occurredOn, command, null);
    }

    public static DefferedCommand CreateDelayed(
        DateTimeOffset occurredOn,
        ICommand command,
        DateTimeOffset processingDate)
    {
        return Create(occurredOn, command, processingDate);
    }

    private static DefferedCommand Create(
        DateTimeOffset occurredOn,
        ICommand command,
        DateTimeOffset? processingDate)
    {
        var message = new DefferedCommand();
        message.Id = Guid.NewGuid();
        message.OccurredOn = occurredOn;
        message.AssemblyName = command.GetType().Assembly.GetName().Name!;
        message.Type = command.GetType().FullName!;

        message.Data = JsonSerializer.Serialize(command);
        message.ProcessingDate = processingDate;

        return message;
    }
}