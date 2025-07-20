using Microsoft.AspNetCore.Mvc;
using Wealth.BuildingBlocks.Application;
using Wealth.CurrencyManagement.API.Controllers.Requests;
using Wealth.CurrencyManagement.Application.Currencies.Commands;
using Wealth.CurrencyManagement.Application.Currencies.Queries;

namespace Wealth.CurrencyManagement.API.Controllers;

[Route("api/[controller]")]
public class CurrencyController : Controller
{
    private readonly ICqrsInvoker invoker;

    public CurrencyController(ICqrsInvoker invoker)
    {
        this.invoker = invoker;
    }

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
            var result = await invoker.Query(new GetCurrencyQuery(id));
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