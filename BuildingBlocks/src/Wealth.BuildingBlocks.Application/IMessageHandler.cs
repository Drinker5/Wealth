namespace Wealth.BuildingBlocks.Application;

public interface IMessageHandler<in T>
    where T : Google.Protobuf.IMessage
{
    Task Handle(T message, CancellationToken token);
}