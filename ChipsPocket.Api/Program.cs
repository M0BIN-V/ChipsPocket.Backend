using ChipsPocket.Api.Endpoints.Auth;
using ChipsPocket.Api.Extensions;
using DiServiceInstaller;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.InstallServices(typeof(Program).Assembly);
builder.AddServiceDefaults();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
    await app.ApplyMigrationsAsync();
}

app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.MapDefaultEndpoints();

app.MapAuthEndpoints();

app.Run();