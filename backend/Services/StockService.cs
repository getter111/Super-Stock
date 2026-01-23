using System.Globalization;
using System.Text.Json;
using StockApi.Models;

namespace StockApi.Services;

public class StockService : IStockService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly ILogger<StockService> _logger;

    public StockService(
        HttpClient httpClient,
        IConfiguration configuration,
        ILogger<StockService> logger)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<List<StockDayData>> GetStockDataAsync(string symbol)
    {
        try
        {
            var apiKey = _configuration["AlphaVantage:ApiKey"];
            var baseUrl = _configuration["AlphaVantage:BaseUrl"];

            var lastMonth = DateTime.Now.AddMonths(-1).ToString("yyyy-MM");

            var url = $"{baseUrl}?function=TIME_SERIES_INTRADAY&symbol={symbol}&interval=15min&month={lastMonth}&outputsize=compact&apikey={apiKey}";

            _logger.LogInformation("Fetching data from Alpha Vantage: {Url}", url);

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();
            
            _logger.LogDebug("API Response: {Response}", jsonString);
            
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var alphaVantageData = JsonSerializer.Deserialize<AlphaVantageResponse>(jsonString, options);

            if (alphaVantageData?.TimeSeries == null || !alphaVantageData.TimeSeries.Any())
            {
                _logger.LogWarning("No time series data found for symbol: {Symbol}. Response might contain error or rate limit message.", symbol);
                
                if (jsonString.Contains("Error Message") || jsonString.Contains("Information") || jsonString.Contains("Note"))
                {
                    _logger.LogError("Alpha Vantage API returned an error/info message: {Message}", jsonString);
                }
                
                return new List<StockDayData>();
            }

            // Group by day and calculate averages
            var groupedByDay = alphaVantageData.TimeSeries
                .GroupBy(kv => DateTime.Parse(kv.Key).Date)
                .Select(group => new StockDayData
                {
                    Day = group.Key.ToString("yyyy-MM-dd"),
                    LowAverage = Math.Round(group.Average(kv => decimal.Parse(kv.Value.Low ?? "0", CultureInfo.InvariantCulture)), 4),
                    HighAverage = Math.Round(group.Average(kv => decimal.Parse(kv.Value.High ?? "0", CultureInfo.InvariantCulture)), 4),
                    Volume = group.Sum(kv => long.Parse(kv.Value.Volume ?? "0", CultureInfo.InvariantCulture))
                })
                .OrderBy(x => x.Day)
                .ToList();

            _logger.LogInformation("Successfully processed {Count} days of data for {Symbol}", groupedByDay.Count, symbol);

            return groupedByDay;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP error while fetching data for symbol: {Symbol}", symbol);
            throw new InvalidOperationException($"Failed to fetch stock data: {ex.Message}", ex);
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "JSON parsing error for symbol: {Symbol}", symbol);
            throw new InvalidOperationException($"Failed to parse API response: {ex.Message}", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error processing data for symbol: {Symbol}", symbol);
            throw;
        }
    }
}
