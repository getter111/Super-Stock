export interface StockDayData {
    day: string;
    lowAverage: number;
    highAverage: number;
    volume: number;
}

export interface ApiError {
    error: string;
}

export interface StockResults {
    data: StockDayData[];
    symbol: string;
}
