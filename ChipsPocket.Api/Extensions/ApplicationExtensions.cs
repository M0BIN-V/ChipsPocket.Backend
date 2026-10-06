using Microsoft.Extensions.FileProviders;

namespace ChipsPocket.Api.Extensions;

public static class ApplicationExtensions
{
    public static WebApplication MapFrontend(this WebApplication app)
    {
        if (app.Environment.IsDevelopment()) return app;
        var staticFilesDirectory = app.Configuration["StaticFilesDirectory"]
                                   ?? throw new InvalidOperationException("Static files directory is missing.");

        var staticFiles = Path.Combine(app.Environment.ContentRootPath, staticFilesDirectory);

        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(staticFiles),

            OnPrepareResponse = context =>
            {
                var path = context.Context.Request.Path.Value ?? string.Empty;

                if (
                    path.Equals("/index.html", StringComparison.OrdinalIgnoreCase) ||
                    path.Equals("/sw.js", StringComparison.OrdinalIgnoreCase) ||
                    path.Equals("/registerSW.js", StringComparison.OrdinalIgnoreCase) ||
                    path.Equals("/manifest.webmanifest", StringComparison.OrdinalIgnoreCase)
                )
                {
                    context.Context.Response.Headers.CacheControl =
                        "no-cache, no-store, must-revalidate";

                    context.Context.Response.Headers.Pragma = "no-cache";
                    context.Context.Response.Headers.Expires = "0";

                    return;
                }

                if (path.StartsWith("/assets/", StringComparison.OrdinalIgnoreCase))
                    context.Context.Response.Headers.CacheControl =
                        "public,max-age=31536000,immutable";
            }
        });

        app.MapFallback(async context =>
        {
            context.Response.ContentType = "text/html";

            context.Response.Headers.CacheControl =
                "no-cache, no-store, must-revalidate";

            context.Response.Headers.Pragma = "no-cache";
            context.Response.Headers.Expires = "0";

            await context.Response.SendFileAsync(
                Path.Combine(staticFiles, "index.html"));
        });


        return app;
    }
}