using System.Text.Json;
using System.Text.Json.Serialization;

namespace Rently.API.Extensions;

public static class ControllerExtensions
{
    public static IServiceCollection ConfigureRentlyControllers(this IServiceCollection services)
    {
        services.AddRouting(options => options.LowercaseUrls = true);

        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            });

        return services;
    }
}
