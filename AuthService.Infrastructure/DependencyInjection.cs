using AuthService.Application.Interfaces;
using AuthService.Infrastructure.Auth;
using AuthService.Infrastructure.Data;
using AuthService.Infrastructure.ExternalServices;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AuthService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            // 1. DbContext.
            services.AddDbContext<AuthDbContext>( opt =>
            opt.UseSqlServer(config.GetConnectionString("AuthDb")));

            // 2. Identity.
            services.AddIdentity<IdentityAppUser, IdentityRole>(
                options =>
                {
                    // For ease of testing, we set some easy password options here.
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequiredLength = 6;
                })
                .AddEntityFrameworkStores<AuthDbContext>()
                .AddDefaultTokenProviders();

            // 3. Application service.
            services.AddScoped<IAuthService, Application.Services.AuthService>();

            // 4. Infrastructure services.
            services.AddScoped<IUserRepository, Repositories.UserRepository>();
            services.Configure<JwtOptions>(config.GetSection("Jwt"));   // bind Jwt section from appsettings.json >> JwtOptions
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            services.AddScoped<IEventPublisher, MassTransitPublisher>();


            // 5. MassTransit for RabbitMQ.
            services.AddMassTransit( x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    var rabbitHost = config["RabbitMQ:HostName"]!;
                    var rabbitUser = config["RabbitMQ:UserName"]!;
                    var rabbitPass = config["RabbitMQ:Password"]!;
                    var rabbitPort = ushort.Parse(config["RabbitMQ:Port"]!);

                    cfg.Host(rabbitHost, rabbitPort, "/", c =>
                    {
                        c.Username(rabbitUser);
                        c.Password(rabbitPass);
                    });
                });
            });

            return services;
        }
    }
}
