using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using StockVision.Entities;

namespace API_SV
{
    public static class TransactionsApi
    {
        public static RouteGroupBuilder MapTransactionsApi(this RouteGroupBuilder api)
        {
            api.RequireAuthorization();
            // select all transactions
            api.MapGet("/", [Authorize] async (ApplicationContext db) =>
            {
                var transactions = await db.Transactions.ToListAsync();
                return Results.Ok(transactions);
            });

            // select transaction by id
            api.MapGet("/id/{id}", [Authorize] async (int id, ApplicationContext db) =>
            {
                var transaction = await db.Transactions.FirstOrDefaultAsync(t => t.TransactionId == id);
                return transaction is null ? Results.NotFound() : Results.Ok(transaction);
            });

            // insert new transaction
            api.MapPost("/", [Authorize] async (Transaction transaction, ApplicationContext db) =>
            {
                db.Transactions.Add(transaction);
                await db.SaveChangesAsync();
                return Results.Ok();
            });

            // update transaction by id
            api.MapPut("/id/{id}", [Authorize] async (int id, Transaction transaction, ApplicationContext db) =>
            {
                var existingTransaction = await db.Transactions.FirstOrDefaultAsync(t => t.TransactionId == id);
                if (existingTransaction is null)
                    return Results.BadRequest($"Transaction with id '{id}' not found");

                existingTransaction.Quantity = transaction.Quantity;
                existingTransaction.TransactionDate = transaction.TransactionDate;
                await db.SaveChangesAsync();
                return Results.Ok();
            });

            // delete transaction by id
            api.MapDelete("/id/{id}", [Authorize] async (int id, ApplicationContext db) =>
            {
                var transaction = await db.Transactions.FirstOrDefaultAsync(t => t.TransactionId == id);
                if (transaction is null)
                    return Results.BadRequest($"Transaction with id '{id}' not found");

                db.Transactions.Remove(transaction);
                await db.SaveChangesAsync();
                return Results.Ok();
            });

            return api;
        }
    }
}
