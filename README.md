
### Backend Setup

1. Navigate to the backend directory:
```bash
cd backend
```

2. Update the API key in `appsettings.json`:
```json
{
  "AlphaVantage": {
    "ApiKey": "YOUR_API_KEY_HERE",
    "BaseUrl": "https://www.alphavantage.co/query"
  }
}
```

3. Restore dependencies and run:
```bash
dotnet restore
dotnet run
```

The API will start at `http://localhost:5000`

- `GET /api/stock/{symbol}` - Get aggregated stock data
- `GET /api/stock/health` - Health check

### Frontend Setup

1. Navigate to the frontend directory:
```bash
cd frontend
```

2. Install dependencies:
```bash
npm install
```

3. Start the development server:
```bash
npm run dev
```

The app will open at `http://localhost:5173`