using Application.QueriesHandler;
using Application.Services;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Infrastructure.Databases.Redis;
using Infrastructure.Databases.SqlServer;
using Infrastructure.Databases.SqlServer.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using WebApi.Extensions;
using static Domain.Utils.Useful;

namespace WebApi
{
    [ExcludeFromCodeCoverage]
    internal static class Program
    {
        static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var redisConnectionString = Environment.GetEnvironmentVariable("REDIS_CONNECTION_STRING")!;
            var sqlServerConnectionString = Environment.GetEnvironmentVariable("SQLSERVER_CONNECTION_STRING")!;


            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<GetAllUsersQueryHandler>());
            builder.Services.AddDbContext<SqlServerContext>(options => options.UseSqlServer(sqlServerConnectionString));
            builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnectionString));
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IAddressRepository, AddressRepository>();
            builder.Services.AddScoped<IRedisRepository, RedisRepository>();
            builder.Services.AddScoped<IAuthService, AuthService>();


            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER"),
                    ValidAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE"),
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_KEY")!))
                };
            });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy(Policies.ADMIN_ONLY, policy => policy.RequireClaim(CustomClaimTypes.Permission, Claims.SYS_ADMIN));
                options.AddPolicy(Policies.USER_OR_ADMIN, policy => policy.RequireClaim(CustomClaimTypes.Permission, Claims.SYS_ADMIN, Claims.SYS_USER));
            });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    policy =>
                    {
                        policy.AllowAnyOrigin()
                              .AllowAnyMethod()
                              .AllowAnyHeader();
                    });
            });
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Customer Onboarding API",
                    Version = "v1",
                    Description = ".NET Core API for Customer Onboarding",
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Insert only JWT Token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });
                c.EnableAnnotations();
            });

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Customer Onboarding API V1");
            });
            app.UseHttpsRedirection();
            app.UseCors("AllowAll");
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.ApplySqlServerMigrations();
            app.SeedSqlServerDatabase();
            app.Run();
        }
    }
}