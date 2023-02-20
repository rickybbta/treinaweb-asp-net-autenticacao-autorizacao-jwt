using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using TWJobs.Core.Data.Contexts;

namespace TWJobs.Core.Config;

public static class IdentityConfig
{
    public static IServiceCollection RegisterIdentity(this IServiceCollection services, IConfiguration configuration)
    {
        var signingKey = configuration.GetValue<string>("Jwt:SigningKey");
        services.AddIdentity<IdentityUser, IdentityRole>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 6;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.User.RequireUniqueEmail = true;
        })
            .AddEntityFrameworkStores<TWJobsDbContext>()
            .AddDefaultTokenProviders();
        services.AddAuthorization();
        services.AddAuthentication(options => {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(options => {
                options.TokenValidationParameters = new TokenValidationParameters{
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("super_secret_key"))
                };
            });

        return services;
    }
}