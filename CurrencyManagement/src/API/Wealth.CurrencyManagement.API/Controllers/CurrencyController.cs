using Microsoft.AspNetCore.Mvc;
using Wealth.CurrencyManagement.API.Controllers.Requests;
using Wealth.CurrencyManagement.Application.Currencies.Commands;
using Wealth.CurrencyManagement.Application.Currencies.Queries;
using Wealth.CurrencyManagement.Domain.Currencies;
using Wealth.CurrencyManagement.Infrastructure;
using Wealth.CurrencyManagement.Infrastructure.Mediation;

namespace Wealth.CurrencyManagement.API.Controllers;

[Route("api/[controller]")]
public class CurrencyController : Controller
{
    private readonly CqrsInvoker invoker;

    public CurrencyController(CqrsInvoker invoker)
    {
        this.invoker = invoker;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var result = await invoker.Query(new GetCurrenciesQuery());
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateCurrencyRequest request)
    {
        var result = await invoker.Command(new CreateCurrencyCommand(request.Id, request.Name, request.Symbol));
        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> Put([FromBody] RenameCurrencyRequest request)
    {
        await invoker.Command(new RenameCurrencyCommand(request.Id, request.NewName));
        return Ok();
    }
}