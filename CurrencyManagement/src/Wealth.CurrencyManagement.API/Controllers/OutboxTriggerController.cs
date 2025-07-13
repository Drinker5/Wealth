using Microsoft.AspNetCore.Mvc;
using Wealth.BuildingBlocks.Infrastructure.Mediation;
using Wealth.CurrencyManagement.Application.Abstractions;
using Wealth.CurrencyManagement.Application.Outbox.Commands;

namespace Wealth.CurrencyManagement.API.Controllers;

[Route("api/[controller]")]
public class OutboxTriggerController : Controller
{
    private readonly ILogger logger;
    private readonly CqrsInvoker cqrsInvoker;
    private readonly IDeferredOperationRepository deferredOperationRepository;

    public OutboxTriggerController(
        ILogger<OutboxTriggerController> logger,
        CqrsInvoker cqrsInvoker,
        IDeferredOperationRepository deferredOperationRepository)
    {
        this.logger = logger;
        this.cqrsInvoker = cqrsInvoker;
        this.deferredOperationRepository = deferredOperationRepository;
    }

    [HttpPost]
    public async Task<IActionResult> Post(Guid outboxMessageId)
    {
        if (outboxMessageId == Guid.Empty)
            return BadRequest("Invalid outbox message id");

        logger.LogInformation("C# ServiceBus queue trigger function processed message: {Item}", outboxMessageId);
        try
        {
            await cqrsInvoker.Command(new ProcessOutboxCommand(outboxMessageId));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Processing OutboxMessage failed");
            return BadRequest("Processing OutboxMessage failed");
        }

        return Ok();
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Next(CancellationToken cancellationToken)
    {
        var unprocessed = await deferredOperationRepository.LoadUnprocessed(1, cancellationToken);
        try
        {
            foreach (var outboxMessageId in unprocessed)
                await cqrsInvoker.Command(new ProcessOutboxCommand(outboxMessageId));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Processing OutboxMessage failed");
            return BadRequest("Processing OutboxMessage failed");
        }

        return Ok();
    }

    private record OutboxMessageReference(Guid OutboxMessageId);
}