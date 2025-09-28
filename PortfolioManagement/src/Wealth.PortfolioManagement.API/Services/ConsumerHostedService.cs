using Confluent.Kafka;
using Polly;
using Polly.Retry;
using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Infrastructure.KafkaConsumer;

namespace Wealth.PortfolioManagement.API.Services;

public sealed class ConsumerHostedService<T> : IHostedService
    where T : Google.Protobuf.IMessage
{
    private readonly IKafkaConsumer consumer;
    private readonly TopicOptions topicOptions;
    private readonly IMessageHandler<T> handler;
    private readonly ILogger<ConsumerHostedService<T>> logger;
    private readonly ResiliencePipeline pipeline;

    public ConsumerHostedService(IKafkaConsumer consumer,
        TopicOptions topicOptions,
        IMessageHandler<T> handler,
        ILogger<ConsumerHostedService<T>> logger)
    {
        this.consumer = consumer;
        this.topicOptions = topicOptions;
        this.handler = handler;
        this.logger = logger;
        pipeline = CreateResiliencePipeline();
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        consumer.ConsumeAsync<T>(
            topicOptions.Topic,
            i => Handle(i, cancellationToken),
            cancellationToken);
        return Task.CompletedTask;
    }

    private Task Handle(ConsumeResult<string, T> result, CancellationToken token)
    {
        return pipeline
            .ExecuteAsync(
                async (t, ct) => await handler.Handle(t, ct),
                result.Message.Value, token)
            .AsTask();
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    private ResiliencePipeline CreateResiliencePipeline()
    {
        // See https://www.pollydocs.org/strategies/retry.html
        var retryOptions = new RetryStrategyOptions
        {
            MaxRetryAttempts = 5,
            DelayGenerator = (context) => ValueTask.FromResult(GenerateDelay(context.AttemptNumber)),
            OnRetry = (args) =>
            {
                var delay = GenerateDelay(args.AttemptNumber) ?? TimeSpan.Zero;
                logger.LogWarning(
                    "Retry attempt {AttemptNumber}. Next retry in {RetryDelay}. Exception: {ExceptionMessage}",
                    args.AttemptNumber + 1,
                    delay,
                    args.Outcome.Exception?.Message);
                
                return ValueTask.CompletedTask;
            }
        };

        return new ResiliencePipelineBuilder()
            .AddRetry(retryOptions)
            .Build();

        static TimeSpan? GenerateDelay(int attempt) => TimeSpan.FromSeconds(Math.Pow(2, attempt));
    }
}