using Microsoft.AspNetCore.Identity;
using Rently.Infrastructure.Data;

namespace Rently.API.Extensions;

public static class IdentityExtensions
{
    public static IServiceCollection ConfigureRentlyIdentity(this IServiceCollection services)
    {
        services.AddIdentity<IdentityUser, IdentityRole>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 6;
            options.Password.RequireUppercase = false;
        })
        .AddEntityFrameworkStores<RentlyDbContext>()
        .AddDefaultTokenProviders();

        return services;
    }
}
