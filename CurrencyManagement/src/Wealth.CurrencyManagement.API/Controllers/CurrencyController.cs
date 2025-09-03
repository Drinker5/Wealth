using Microsoft.AspNetCore.Mvc;
using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.CurrencyManagement.API.Controllers.Requests;
using Wealth.CurrencyManagement.Application.Currencies.Commands;
using Wealth.CurrencyManagement.Application.Currencies.Queries;

namespace Wealth.CurrencyManagement.API.Controllers;

[Route("api/[controller]")]
public class CurrencyController(ICqrsInvoker invoker) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var result = await invoker.Query(new GetCurrenciesQuery());
        return Ok(result);
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        try
        {
            if (!CurrencyId.TryParse(id, out var currencyId))
                return NotFound("Cannot parse currency");
                
            var result = await invoker.Query(new GetCurrencyQuery(currencyId));
            if (result is null)
                return NotFound("Currency not found");

            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
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