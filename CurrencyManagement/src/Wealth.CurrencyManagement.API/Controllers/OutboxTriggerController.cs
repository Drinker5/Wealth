using Microsoft.AspNetCore.Mvc;
using Wealth.BuildingBlocks.Application;
using Wealth.CurrencyManagement.Application.Abstractions;
using Wealth.CurrencyManagement.Application.Outbox.Commands;

namespace Wealth.CurrencyManagement.API.Controllers;

[Route("api/[controller]")]
public class OutboxTriggerController : Controller
{
    private readonly ILogger logger;
    private readonly ICqrsInvoker cqrsInvoker;
    private readonly IDeferredOperationRepository deferredOperationRepository;

    public OutboxTriggerController(
        ILogger<OutboxTriggerController> logger,
        ICqrsInvoker cqrsInvoker,
        IDeferredOperationRepository deferredOperationRepository)
    {
        this.logger = logger;
        this.cqrsInvoker = cqrsInvoker;
        this.deferredOperationRepository = deferredOperationRepository;
    }

    [HttpPost]
    public async Task<IActionResult> Post(Guid operationId)
    {
        if (operationId == Guid.Empty)
            return BadRequest("Invalid operation Id");

        logger.LogInformation("C# ServiceBus queue trigger function processed message: {Item}", operationId);
        try
        {
            await cqrsInvoker.Command(new ProcessDeferredOperationCommand(operationId));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Processing failed");
            return BadRequest("Processing failed");
        }

        return Ok();
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Next(CancellationToken cancellationToken)
    {
        var unprocessed = await deferredOperationRepository.LoadUnprocessed(1, cancellationToken);
        try
        {
            foreach (var operationId in unprocessed)
                await cqrsInvoker.Command(new ProcessDeferredOperationCommand(operationId));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Processing failed");
            return BadRequest("Processing failed");
        }

        return Ok();
    }
}