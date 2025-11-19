using Microsoft.AspNetCore.Mvc;
using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.CurrencyManagement.API.Controllers.Requests;
using Wealth.CurrencyManagement.Application.ExchangeRates.Commands;
using Wealth.CurrencyManagement.Application.ExchangeRates.Queries;

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
        await invoker.Command(new CreateExchangeRateCommand(request.From, request.To, request.Rate, request.Date));
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> Exchange(
        [FromQuery] CurrencyCode from,
        [FromQuery] decimal value,
        [FromQuery] CurrencyCode to,
        [FromQuery] DateOnly date)
    {
        var result = await invoker.Query(new ExchangeQuery(new Money(from, value), to, date));
        return Ok(result);
    }


    [HttpPost("[action]/{from}/{to}")]
    public async Task<IActionResult> CheckNewExchangeRates(CurrencyCode from, CurrencyCode to)
    {
        await invoker.Command(new CheckNewExchangeRatesCommand(from, to));
        return Ok();
    }
}