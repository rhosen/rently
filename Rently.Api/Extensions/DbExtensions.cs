using Microsoft.EntityFrameworkCore;
using Rently.Infrastructure.Data;

namespace Rently.API.Extensions;

public static class DbExtensions
{
    public static IServiceCollection ConfigureRentlyDatabase(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<RentlyDbContext>(options =>
            options.UseSqlServer(config.GetConnectionString("DefaultConnection")));
        return services;
    }
}
