import { useState } from 'react';
import { stockApi } from './api';
import type { StockDayData } from './types';
import StockResults from './components/StockResults';

export default function App() {
    const [symbol, setSymbol] = useState('');
    const [searchedSymbol, setSearchedSymbol] = useState('');
    const [stockData, setStockData] = useState<StockDayData[]>([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        
        if (!symbol.trim()) {
            setError('Please enter a stock symbol');
            return;
        }

        setLoading(true);
        setError(null);
        setStockData([]);

        try {
            const upperSymbol = symbol.toUpperCase();
            const data = await stockApi.getStockData(upperSymbol);
            setStockData(data);
            setSearchedSymbol(upperSymbol);
            
            if (data.length === 0) {
                setError('No data found for this symbol');
            }
        } catch (err: any) {
            console.error('Error fetching stock data:', err);
            setError(
                err.response?.data?.error || 
                'Failed to fetch stock data. Please check the symbol and try again.'
            );  
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="min-h-screen bg-gray-50 flex flex-col">
            <header className="bg-gradient-to-r from-blue-600 to-blue-800 text-white py-8 px-4 shadow-lg">
                <div className="max-w-6xl mx-auto">
                    <h1 className="text-3xl font-bold">Stock Data Viewer</h1>
                </div>
            </header>

            <main className="flex-1 max-w-6xl w-full mx-auto px-4 py-8 space-y-6">
                <div className="bg-white rounded-lg shadow p-6">
                    <form onSubmit={handleSubmit} className="max-w-2xl mx-auto">
                        <div className="flex gap-4">
                            <input
                                type="text"
                                value={symbol}
                                onChange={(e) => setSymbol(e.target.value.toUpperCase())}
                                placeholder="Enter stock symbol (e.g., IBM, AAPL)"
                                className="flex-1 px-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent outline-none font-mono uppercase disabled:bg-gray-100"
                                disabled={loading}
                            />
                            <button 
                                type="submit" 
                                className="px-8 py- cursor-pointer bg-blue-600 text-white rounded-lg font-semibold hover:bg-blue-700 disabled:opacity-50 disabled:cursor-not-allowed transition"
                                disabled={loading}
                            >
                                {loading ? 'Loading...' : 'Search'}
                            </button>
                        </div>
                    </form>

                    {error && (
                        <div className="mt-4 max-w-2xl mx-auto p-4 bg-red-50 border border-red-200 rounded-lg text-red-700">
                        ⚠️ {error}
                        </div>
                    )}
                </div>

                {stockData.length > 0 && (
                    <StockResults data={stockData} symbol={searchedSymbol} />
                )}
            </main>
        </div>
    );
}
