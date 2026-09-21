using ChipsPocket.Api.Endpoints.Auth;
using ChipsPocket.Api.Endpoints.Table;
using ChipsPocket.Api.Extensions;
using DiServiceInstaller;
using FluentValidation;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

var assembly = typeof(Program).Assembly;

builder.InstallServices(assembly);
builder.AddServiceDefaults();
builder.Services.AddValidatorsFromAssembly(assembly);


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
    await app.ApplyMigrationsAsync();
}

app.UseCors("Web");
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.MapDefaultEndpoints();

app.MapAuthEndpoints();
app.MapTablesEndpoints();

app.Run();