using Microsoft.AspNetCore.Mvc;
using StockApi.Services;
using StockApi.Models;

namespace StockApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StockController : ControllerBase
{
    private readonly IStockService _stockService;
    private readonly ILogger<StockController> _logger;

    public StockController(IStockService stockService, ILogger<StockController> logger)

    {
        _stockService = stockService;
        _logger = logger;
    }

    [HttpGet("{symbol}")]
    [ProducesResponseType(typeof(List<StockDayData>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetStockData(string symbol)
    {
        if (string.IsNullOrWhiteSpace(symbol))
        {
            return BadRequest(new { error = "Symbol parameter is required" });
        }

        try
        {
            _logger.LogInformation("Received request for symbol: {Symbol}", symbol);
            
            var data = await _stockService.GetStockDataAsync(symbol.ToUpperInvariant());
            
            if (!data.Any())
            {
                return NotFound(new { error = $"No data found for symbol: {symbol}" });
            }

            return Ok(data);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Error processing request for symbol: {Symbol}", symbol);
            return StatusCode(StatusCodes.Status502BadGateway, new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error processing request for symbol: {Symbol}", symbol);
            return StatusCode(StatusCodes.Status500InternalServerError, 
                new { error = "An unexpected error occurred" });
        }
    }

    [HttpGet("health")]
    public IActionResult HealthCheck()
    {
        return Ok(new { status = "healthy", timestamp = DateTime.UtcNow });
    }
}
