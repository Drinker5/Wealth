using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Application.CommandScheduler;
using Wealth.BuildingBlocks.Domain.Utilities;

namespace Wealth.CurrencyManagement.Application.Tests.TestHelpers;

public class OutboxMessageBuilder
{
    private record struct EmptyCommand : ICommand;
    private DateTimeOffset date = Clock.Now;
    private ICommand obj = new EmptyCommand();
    private DateTimeOffset? processingDate = null;

    public DefferedCommand Build()
    {
        if (processingDate.HasValue)
            return DefferedCommand.CreateDelayed(date, obj, processingDate.Value);
        
        return DefferedCommand.Create(date, obj);
    }

    public OutboxMessageBuilder SetProcessingDate(DateTimeOffset newProcessingDate)
    {
        this.processingDate = newProcessingDate;
        return this;
    }
    
    public OutboxMessageBuilder SetDate(DateTimeOffset newDate)
    {
        this.date = newDate;
        return this;
    }

    public OutboxMessageBuilder SetObject(ICommand newObj)
    {
        this.obj = newObj;
        return this;
    }
}