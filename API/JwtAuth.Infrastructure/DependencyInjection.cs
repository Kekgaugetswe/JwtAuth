using JwtAuth.Domain.Interfaces;
using JwtAuth.Infrastructure.DataContext;
using JwtAuth.Infrastructure.Repositories;
using JwtAuth.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace JwtAuth.Infrastructure;

public  static class DependencyInjection
{
    public static IServiceCollection AddAuthenticationServices(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddIdentityCore<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("JwtAuth")
                .AddEntityFrameworkStores<AuthDbContext>()
                .AddDefaultTokenProviders();


        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit =  false;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.Password.RequiredUniqueChars = 1;
        options.Password.RequiredLength = 6;
        });

        services.AddScoped<ITokenRepository, TokenRepository>();
        services.AddTransient<IEmailSender,EmailSender>();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer( options => {
            options.TokenValidationParameters = new TokenValidationParameters{
                AuthenticationType = "Jwt",
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidAudience =configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(configuration["Jwt:key"]))
            };
        });
        services.AddDbContext<AuthDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
        );

        return services;
    }
}
