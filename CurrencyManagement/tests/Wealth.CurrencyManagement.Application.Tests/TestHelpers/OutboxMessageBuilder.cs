using Wealth.BuildingBlocks.Domain.Utilities;
using Wealth.CurrencyManagement.Application.Abstractions;

namespace Wealth.CurrencyManagement.Application.Tests.TestHelpers;

public class OutboxMessageBuilder
{
    private DateTimeOffset date = Clock.Now;
    private IJsonSerializer serializer = Substitute.For<IJsonSerializer>();
    private object obj = new Object();
    private DateTimeOffset? processingDate = null;

    public OutboxMessage Build()
    {
        serializer.Serialize(Arg.Any<object?>()).Returns(string.Empty);

        if (processingDate.HasValue)
            return OutboxMessage.CreateDelayed(serializer, date, obj, processingDate.Value);
        
        return OutboxMessage.Create(serializer, date, obj);
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

    public OutboxMessageBuilder SetObject(object newObj)
    {
        this.obj = newObj;
        return this;
    }

    public OutboxMessageBuilder SetSerializer(IJsonSerializer newSerializer)
    {
        this.serializer = newSerializer;
        return this;
    }
}