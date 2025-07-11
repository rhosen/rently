namespace Rently.API.Extensions;

public static class CorsExtensions
{
    public static IServiceCollection ConfigureRentlyCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowFrontend", policy =>
            {
                policy.WithOrigins("https://localhost:7091")
                      .AllowAnyHeader()
                      .AllowAnyMethod();
            });
        });

        return services;
    }
}
