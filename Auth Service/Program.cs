using Auth_Service;
using Auth_Service.Data;
using Auth_Service.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddDbContext<AuthDb>(opt => 
    opt.UseNpgsql(builder.Configuration.GetConnectionString("financedb"))
);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHsts();
app.UseHttpsRedirection();

using (var scope = app.Services.CreateScope())
{
    var dbCtx = scope.ServiceProvider.GetRequiredService<AuthDb>();
    dbCtx.Database.EnsureCreated();

    try
    {
        dbCtx.Users.Count();
    }
    catch (Exception)
    {
        var dbCreator = dbCtx.GetService<IRelationalDatabaseCreator>();
        dbCreator.CreateTables();
    }
}

app.MapGet("/", Auth.IdFromToken).Produces<int>(200).Produces(401);
app.MapPost("/", Auth.Authenticate).Produces<Token>(200).Produces(401);
app.MapPost("/register", Auth.Register);

app.Run();
