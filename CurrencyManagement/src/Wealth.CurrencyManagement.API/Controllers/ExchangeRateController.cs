using Microsoft.AspNetCore.Mvc;
using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.CurrencyManagement.API.Controllers.Requests;
using Wealth.CurrencyManagement.Application.ExchangeRates.Commands;
using Wealth.CurrencyManagement.Application.ExchangeRates.Queries;
using Money = Wealth.CurrencyManagement.Domain.ExchangeRates.Money;

namespace Wealth.CurrencyManagement.API.Controllers;

[Route("api/[controller]")]
public class ExchangeRateController : Controller
{
    private readonly ICqrsInvoker invoker;

    public ExchangeRateController(ICqrsInvoker invoker)
    {
        this.invoker = invoker;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateEchangeRateRequest request)
    {
        await invoker.Command(new CreateExchangeRateCommand(request.FromId, request.ToId, request.Rate, request.Date));
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> Exchange([FromQuery] string fromId, [FromQuery] decimal value, [FromQuery] string toId, [FromQuery] DateOnly date)
    {
        var result = await invoker.Query(new ExchangeQuery(new Money(fromId, value), new CurrencyId(toId), date));
        return Ok(result);
    }

    
    [HttpPost("[action]/{fromId}/{toId}")]
    public async Task<IActionResult> CheckNewExchangeRates(string fromId, string toId)
    {
        await invoker.Command(new CheckNewExchangeRatesCommand(new CurrencyId(fromId), new CurrencyId(toId)));
        return Ok();
    }
}