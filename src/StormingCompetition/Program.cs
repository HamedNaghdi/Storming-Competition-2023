using Microsoft.EntityFrameworkCore;
using StormingCompetition.Models;
using System;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataContext>(ServiceLifetime.Scoped);

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

//Auto DB Migration
// Migrate latest database changes during startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider
        .GetRequiredService<DataContext>();

    // Here is the migration executed
    dbContext.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
