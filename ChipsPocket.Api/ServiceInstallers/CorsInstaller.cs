using DiServiceInstaller;

namespace ChipsPocket.Api.ServiceInstallers;

public class CorsInstaller : IServiceInstaller
{
    public void Install(IHostApplicationBuilder builder)
    {
        
        if (builder.Environment.IsDevelopment())
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("Web", policy =>
                {
                    policy
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
    }
}