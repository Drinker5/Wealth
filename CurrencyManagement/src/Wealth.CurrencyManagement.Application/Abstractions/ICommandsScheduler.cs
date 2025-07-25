﻿using Wealth.BuildingBlocks.Application;

namespace Wealth.CurrencyManagement.Application.Abstractions;

public interface ICommandsScheduler
{
    Task EnqueueAsync(ICommand command, CancellationToken cancellationToken = default);
    Task EnqueueAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default);
    Task EnqueuePublishingEventAsync(OutboxMessage integrationEvent, CancellationToken cancellationToken = default);
    Task ScheduleAsync(ICommand command, DateTimeOffset date, CancellationToken cancellationToken = default);
}