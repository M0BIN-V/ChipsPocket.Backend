using ChipsPocket.Api.Endpoints.Auth;
using ChipsPocket.Api.Endpoints.Table;
using ChipsPocket.Api.Extensions;
using ChipsPocket.Api.Realtime;
using DiServiceInstaller;
using FluentValidation;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

var assembly = typeof(Program).Assembly;

builder.InstallServices(assembly);
builder.AddServiceDefaults();
builder.Services.AddValidatorsFromAssembly(assembly);
builder.Services.AddRealtime();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
    await app.ApplyMigrationsAsync();
    await app.SeedDataAsync();
}

app.Services
    .GetRequiredService<RealtimeRegistry>()
    .AddTableRealtime();

app.UseCors("Web");
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.MapDefaultEndpoints();

app.MapAuthEndpoints();
app.MapTablesEndpoints();

app.MapRealtimeHub<TableHub>(
    "/hubs/table",
    "Provides realtime communication for poker tables.");

app.MapRealtimeManifest();

app.Run();