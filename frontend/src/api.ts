import axios from 'axios';
import type { StockDayData } from './types';

const API_BASE_URL = 'http://localhost:5000/api';

export const stockApi = {
    async getStockData(symbol: string): Promise<StockDayData[]> {
        const response = await axios.get<StockDayData[]>(`${API_BASE_URL}/stock/${symbol}`);
        return response.data;
    },

    async healthCheck(): Promise<{ status: string; timestamp: string }> {
        const response = await axios.get(`${API_BASE_URL}/stock/health`);
        return response.data;
    }
};
