using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using StockVision.Entities;

namespace API_SV
{
    public static class UsersApi
    {
        public static RouteGroupBuilder MapUsersApi(this RouteGroupBuilder api)
        {
            api.RequireAuthorization();
            // select all users
            api.MapGet("/", [Authorize] async (ApplicationContext db) =>
            {
                var users = await db.Users.ToListAsync();
                return Results.Ok(users);
            });

            // select user by name
            api.MapGet("/name/{name}", [Authorize] async (string name, ApplicationContext db) =>
            {
                var user = await db.Users.FirstOrDefaultAsync(u => u.Name == name);
                return user is null ? Results.NotFound() : Results.Ok(user);
            });

            // insert new user
            api.MapPost("/", [Authorize] async (User user, ApplicationContext db) =>
            {
                ArgumentNullException.ThrowIfNull(user?.Name);
                db.Users.Add(user);
                await db.SaveChangesAsync();
                return Results.Ok();
            });

            // update user by name
            api.MapPut("/name/{name}", [Authorize] async (string name, User user, ApplicationContext db) =>
            {
                var existingUser = await db.Users.FirstOrDefaultAsync(u => u.Name == name);
                if (existingUser is null)
                    return Results.BadRequest($"User '{name}' not found");

                existingUser.Name = user.Name;
                await db.SaveChangesAsync();
                return Results.Ok();
            });

            // delete user by name
            api.MapDelete("/name/{name}", [Authorize] async (string name, ApplicationContext db) =>
            {
                var user = await db.Users.FirstOrDefaultAsync(u => u.Name == name);
                if (user is null)
                    return Results.BadRequest($"User '{name}' not found");

                db.Users.Remove(user);
                await db.SaveChangesAsync();
                return Results.Ok();
            });

            return api;
        }
    }
}
