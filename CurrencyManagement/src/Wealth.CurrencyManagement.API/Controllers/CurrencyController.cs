using Microsoft.AspNetCore.Mvc;
using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;
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
    public async Task<IActionResult> Get(CurrencyCode id)
    {
        try
        {
            if (id == CurrencyCode.None)
                return BadRequest("Cannot parse currency");
                
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
}