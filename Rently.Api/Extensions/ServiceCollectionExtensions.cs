namespace Rently.API.Extensions;

public static class ServiceCollectionExtensions
{

    public static IServiceCollection ConfigureServiceCollection(this IServiceCollection services, IConfiguration config)
    {
        services.ConfigureRentlyAppServices(config);
        services.ConfigureRentlyControllers();
        services.ConfigureRentlyCors();
        services.ConfigureRentlyDatabase(config);
        services.ConfigureRentlyIdentity();
        services.ConfigureRentlyJwt(config);
        services.ConfigureRentlySwagger();
        return services;
    }
}
