using ChipsPocket.Api.Endpoints.Auth;
using ChipsPocket.Api.Extensions;
using DiServiceInstaller;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.InstallServices(typeof(Program).Assembly);
builder.AddServiceDefaults();
builder.Services.AddOpenApi();
builder.Services.AddCors(options =>
{
    options.AddPolicy("Web", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

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

app.Run();