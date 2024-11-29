using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using StockVision.Entities;

namespace API_SV
{
    public static class AssetsApi
    {
        public static RouteGroupBuilder MapAssetsApi(this RouteGroupBuilder api)
        {
            api.RequireAuthorization();
            // select all assets
            api.MapGet("/", [Authorize] async (ApplicationContext db) =>
            {
                var assets = await db.Assets.ToListAsync();
                return Results.Ok(assets);
            });

            // Добавить метод для поиска активов с фильтрацией по символу и диапазону цен
            api.MapGet("/search", [Authorize] async (string? symbol, decimal? priceFrom, decimal? priceTo, ApplicationContext db) =>
            {
                // Создаём базовый запрос
                var query = db.Assets.AsQueryable();

                // Фильтруем по символу, если задан
                if (!string.IsNullOrWhiteSpace(symbol))
                {
                    query = query.Where(a => a.Symbol.Contains(symbol));
                }

                // Фильтруем по диапазону цен, если задан
                if (priceFrom.HasValue)
                {
                    query = query.Where(a => a.CurrentPrice >= priceFrom.Value);
                }

                if (priceTo.HasValue)
                {
                    query = query.Where(a => a.CurrentPrice <= priceTo.Value);
                }

                // Выполняем запрос
                var filteredAssets = await query.ToListAsync();

                return Results.Ok(filteredAssets);
            });


            // select asset by symbol
            api.MapGet("/symbol/{symbol}", [Authorize] async (string symbol, ApplicationContext db) =>
            {
                var asset = await db.Assets.FirstOrDefaultAsync(a => a.Symbol == symbol);
                return asset is null ? Results.NotFound() : Results.Ok(asset);
            });

            // insert new asset
            api.MapPost("/", [Authorize] async (Asset asset, ApplicationContext db) =>
            {
                ArgumentNullException.ThrowIfNull(asset?.Symbol);
                db.Assets.Add(asset);
                await db.SaveChangesAsync();
                return Results.Ok();
            });


            // update asset by symbol
            api.MapPut("/symbol/{symbol}", [Authorize] async (string symbol, Asset asset, ApplicationContext db) =>
            {
                var existingAsset = await db.Assets.FirstOrDefaultAsync(a => a.Symbol == symbol);
                if (existingAsset is null)
                    return Results.BadRequest($"Asset '{symbol}' not found");

                // Update all the properties
                existingAsset.Type = asset.Type;
                existingAsset.Symbol = asset.Symbol;
                existingAsset.CurrentPrice = asset.CurrentPrice;
                existingAsset.Currency = asset.Currency;
                existingAsset.Exchange = asset.Exchange;
                existingAsset.LastUpdated = asset.LastUpdated;
                existingAsset.UpdateDate = asset.UpdateDate;

                // Mark entity as updated
                db.Assets.Update(existingAsset);
                await db.SaveChangesAsync();
                return Results.Ok();
            });

            // delete asset by symbol
            api.MapDelete("/symbol/{symbol}", [Authorize] async (string symbol, ApplicationContext db) =>
            {
                var asset = await db.Assets.FirstOrDefaultAsync(a => a.Symbol == symbol);
                if (asset is null)
                    return Results.BadRequest($"Asset '{symbol}' not found");

                db.Assets.Remove(asset);
                await db.SaveChangesAsync();
                return Results.Ok();
            });

            return api;
        }
    }
}
