using CommodityPriceManager.ApiService.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();

builder.Services.AddOpenApi();

var connectionString = builder.Configuration.GetConnectionString("sqldb") 
    ?? builder.Configuration["ConnectionStrings__sqldb"] 
    ?? builder.Configuration["DATABASE_URL"];

if (!string.IsNullOrEmpty(connectionString) && connectionString.StartsWith("postgresql://"))
{
    // Convert postgresql://user:pass@host:port/db to Npgsql format
    var uri = new Uri(connectionString);
    var userInfo = uri.UserInfo.Split(':');
    connectionString = $"Host={uri.Host};Port={uri.Port};Database={uri.AbsolutePath.Trim('/')};Username={userInfo[0]};Password={userInfo[1]};SSL Mode=Require;Trust Server Certificate=true";
}

Console.WriteLine($"[DEBUG] Using Connection String: {connectionString?.Substring(0, Math.Min(connectionString.Length, 20))}...");

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Connection string 'sqldb' not found.");
}

builder.AddNpgsqlDbContext<AppDbContext>("sqldb", configureSettings: settings => {
    settings.ConnectionString = connectionString;
});

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();
app.MapDefaultEndpoints();
app.UseCors();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

app.MapGet("/categories", async (AppDbContext db) =>
{
    return await db.Categories.ToListAsync();
});

app.MapPost("/categories", async (Category category, AppDbContext db) =>
{
    db.Categories.Add(category);
    await db.SaveChangesAsync();
    return Results.Created($"/categories/{category.Id}", category);
});

app.MapGet("/commodities", async (AppDbContext db) =>
{
    return await db.Commodities.Include(c => c.Category).ToListAsync();
});

app.MapPost("/commodities", async (Commodity commodity, AppDbContext db) =>
{
    db.Commodities.Add(commodity);
    // Add initial price history
    db.PriceHistories.Add(new PriceHistory 
    { 
        Commodity = commodity, 
        Price = commodity.CurrentPrice, 
        UpdatedAt = DateTime.UtcNow 
    });
    await db.SaveChangesAsync();
    return Results.Created($"/commodities/{commodity.Id}", commodity);
});

app.MapPut("/commodities/{id:int}", async (int id, Commodity updatedCommodity, AppDbContext db) =>
{
    var commodity = await db.Commodities.FindAsync(id);
    if (commodity is null) return Results.NotFound();

    if (commodity.CurrentPrice != updatedCommodity.CurrentPrice)
    {
        db.PriceHistories.Add(new PriceHistory
        {
            CommodityId = commodity.Id,
            Price = updatedCommodity.CurrentPrice,
            UpdatedAt = DateTime.UtcNow
        });
    }

    commodity.Name = updatedCommodity.Name;
    commodity.CategoryId = updatedCommodity.CategoryId;
    commodity.CurrentPrice = updatedCommodity.CurrentPrice;

    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapGet("/commodities/{id:int}/history", async (int id, AppDbContext db) =>
{
    return await db.PriceHistories
        .Where(x => x.CommodityId == id)
        .OrderByDescending(x => x.UpdatedAt)
        .ToListAsync();
});

app.Run();

