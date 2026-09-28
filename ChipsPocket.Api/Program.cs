using System.Text.Json.Serialization;
using ChipsPocket.Api.Endpoints.Auth;
using ChipsPocket.Api.Endpoints.Chips;
using ChipsPocket.Api.Endpoints.Table;
using ChipsPocket.Api.Extensions;
using ChipsPocket.Api.Realtime;
using DiServiceInstaller;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

var assembly = typeof(Program).Assembly;

builder.InstallServices(assembly);
builder.AddServiceDefaults();
builder.Services.AddValidatorsFromAssembly(assembly);
builder.Services.AddRealtime();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

var test = app.Configuration["Test"];

Console.WriteLine("this is test test test test test "+test);

await app.ApplyMigrationsAsync();
await app.SeedDataAsync();

app.Services
    .GetRequiredService<RealtimeRegistry>()
    .AddTableRealtime();

app.UseCors("Web");
app.UseAuthentication();
app.UseAuthorization();
if (!app.Environment.IsDevelopment())
    app.UseHttpsRedirection();
app.MapDefaultEndpoints();

app.MapAuthEndpoints();
app.MapTablesEndpoints();
app.MapChipsEndpoints();

app.MapRealtimeHub<TableHub>(
    "/hubs/table",
    "Provides realtime communication for poker tables.");

app.MapRealtimeManifest();

app.Run();