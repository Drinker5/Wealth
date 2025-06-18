using Microsoft.AspNetCore.Mvc;
using Wealth.CurrencyManagement.Application.Currencies.Queries;
using Wealth.CurrencyManagement.Application.ExchangeRates.Commands;
using Wealth.CurrencyManagement.Application.ExchangeRates.Query;
using Wealth.CurrencyManagement.Domain.Currencies;
using Wealth.CurrencyManagement.Domain.ExchangeRates;
using Wealth.CurrencyManagement.Infrastructure;
using Wealth.CurrencyManagement.Infrastructure.Mediation;

namespace Wealth.CurrencyManagement.API.Controllers;

[Route("api/[controller]")]
public class ExchangeRateController : Controller
{
    private readonly CqrsInvoker invoker;

    public ExchangeRateController(CqrsInvoker invoker)
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
}