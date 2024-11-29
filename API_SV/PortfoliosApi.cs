using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using StockVision.Entities;

namespace API_SV
{
    public static class PortfoliosApi
    {
        public static RouteGroupBuilder MapPortfoliosApi(this RouteGroupBuilder api)
        {
            api.RequireAuthorization();
            // select all portfolios
            api.MapGet("/", [Authorize] async (ApplicationContext db) =>
            {
                var portfolios = await db.Portfolios.ToListAsync();
                return Results.Ok(portfolios);
            });

            // select portfolio by id
            api.MapGet("/id/{id}", [Authorize] async (int id, ApplicationContext db) =>
            {
                var portfolio = await db.Portfolios.FirstOrDefaultAsync(p => p.PortfolioId == id);
                return portfolio is null ? Results.NotFound() : Results.Ok(portfolio);
            });

            // insert new portfolio
            api.MapPost("/", [Authorize] async (Portfolio portfolio, ApplicationContext db) =>
            {
                ArgumentNullException.ThrowIfNull(portfolio?.Name);
                db.Portfolios.Add(portfolio);
                await db.SaveChangesAsync();
                return Results.Ok();
            });

            // update portfolio by id
            api.MapPut("/id/{id}", [Authorize] async (int id, Portfolio portfolio, ApplicationContext db) =>
            {
                var existingPortfolio = await db.Portfolios.FirstOrDefaultAsync(p => p.PortfolioId == id);
                if (existingPortfolio is null)
                    return Results.BadRequest($"Portfolio with id '{id}' not found");

                // Обновляем все нужные поля
                existingPortfolio.Name = portfolio.Name;
                existingPortfolio.PortfolioName = portfolio.PortfolioName;
                existingPortfolio.UserId = portfolio.UserId;
                existingPortfolio.TotalValue = portfolio.TotalValue;
                existingPortfolio.LastUpdated = portfolio.LastUpdated;

                await db.SaveChangesAsync();
                return Results.Ok();
            });

            // delete portfolio by id
            api.MapDelete("/id/{id}", [Authorize] async (int id, ApplicationContext db) =>
            {
                var portfolio = await db.Portfolios.FirstOrDefaultAsync(p => p.PortfolioId == id);
                if (portfolio is null)
                    return Results.BadRequest($"Portfolio with id '{id}' not found");

                db.Portfolios.Remove(portfolio);
                await db.SaveChangesAsync();
                return Results.Ok();
            });

            return api;
        }
    }
}
