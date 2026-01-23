namespace StockApi.Models;

public class StockDayData
{
    public string Day { get; set; } = string.Empty;
    public decimal LowAverage { get; set; }
    public decimal HighAverage { get; set; }
    public long Volume { get; set; }
}
