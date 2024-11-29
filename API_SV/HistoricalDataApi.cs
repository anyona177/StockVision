using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using StockVision.Entities;

namespace API_SV
{
    public static class HistoricalDataApi
    {
        public static RouteGroupBuilder MapHistoricalDataApi(this RouteGroupBuilder api)
        {
            api.RequireAuthorization();
            api.MapGet("/", [Authorize] async (ApplicationContext db) =>
            {
                var historicalData = await db.HistoricalData.Include(h => h.Asset).ToListAsync();
                return Results.Ok(historicalData);
            });

            api.MapGet("/symbol/{symbol}", [Authorize] async (string symbol, ApplicationContext db) =>
            {
                var data = await db.HistoricalData
                    .Include(h => h.Asset)
                    .FirstOrDefaultAsync(h => h.Asset != null && h.Asset.Symbol == symbol);
                return data is null ? Results.NotFound() : Results.Ok(data);
            });

            api.MapPost("/", [Authorize] async (HistoricalData historicalData, ApplicationContext db) =>
            {
                db.HistoricalData.Add(historicalData);
                await db.SaveChangesAsync();
                return Results.Ok();
            });

            api.MapGet("/filter", [Authorize] async (string? ticker, string? exchange, decimal? priceFrom, decimal? priceTo, int? volumeFrom, int? volumeTo, ApplicationContext db) =>
            {
                // Создаем запрос с включением связанных активов
                var query = db.HistoricalData.Include(h => h.Asset).AsQueryable();

                // Применяем фильтры, если они заданы
                if (!string.IsNullOrEmpty(ticker))
                    query = query.Where(h => h.Asset != null && h.Asset.Symbol == ticker);

                if (!string.IsNullOrEmpty(exchange))
                    query = query.Where(h => h.Asset != null && h.Asset.Exchange == exchange);

                if (priceFrom.HasValue)
                    query = query.Where(h => h.ClosingPrice >= priceFrom.Value);

                if (priceTo.HasValue)
                    query = query.Where(h => h.ClosingPrice <= priceTo.Value);

                if (volumeFrom.HasValue)
                    query = query.Where(h => h.Volume >= volumeFrom.Value);

                if (volumeTo.HasValue)
                    query = query.Where(h => h.Volume <= volumeTo.Value);

                // Выполняем запрос и возвращаем результат
                var result = await query.ToListAsync();
                return Results.Ok(result);
            });

            return api;
        }
    }
}
