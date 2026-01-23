using StockApi.Models;

namespace StockApi.Services;

public interface IStockService
{
    Task<List<StockDayData>> GetStockDataAsync(string symbol);
}
