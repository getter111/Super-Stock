import type { StockResults } from '../types';

export default function StockResults({ data, symbol }: StockResults) {
    const formatNumber = (num: number) => {
        return num.toLocaleString('en-US', { 
            minimumFractionDigits: 2, 
            maximumFractionDigits: 4 
        });
    };

    return (
        <div className="bg-white rounded-lg shadow p-6">
            <h2 className="text-xl font-semibold mb-4">
                Results for {symbol}{' '}
            </h2>
        
            <div className="overflow-x-auto">
                <table className="min-w-full divide-y divide-gray-200">
                    <thead className="bg-gray-50">
                        <tr>
                            <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                                Date
                            </th>

                            <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                                Low Average
                            </th>

                            <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                                High Average
                            </th>

                            <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                                Volume
                            </th>
                        </tr>
                    </thead>

                    <tbody className="bg-white divide-y divide-gray-200">
                        {data.map((day) => (
                            <tr key={day.day} className="hover:bg-gray-50">
                                <td className="px-6 py-4 whitespace-nowrap text-sm font-mono font-medium text-gray-900">
                                    {day.day}
                                </td>

                                <td className="px-6 py-4 whitespace-nowrap text-sm font-mono text-gray-900">
                                    ${formatNumber(day.lowAverage)}
                                </td>

                                <td className="px-6 py-4 whitespace-nowrap text-sm font-mono text-gray-900">
                                    ${formatNumber(day.highAverage)}
                                </td>
                                
                                <td className="px-6 py-4 whitespace-nowrap text-sm font-mono text-gray-900">
                                    {formatNumber(day.volume)}
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            </div>
        </div>
    );
}
