
using KarateClub.Application.Handlers.User;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using KarateClub.Infrastructure;
using KarateClub.Application.Security;
using KarateClub.Api.Middlewares;
using KarateClub.Infrastructure.Persistence;
using KarateClub.Application.Interfaces.Repositories;
using KarateClub.Infrastructure.Persistence.Repositories;
using KarateClub.Infrastructure.Authorization;
using KarateClub.Application.Handlers.Person;

namespace KarateClub.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "Enter: Bearer {your token}"
                });

                options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
                    {
                        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                        {
                            Reference = new Microsoft.OpenApi.Models.OpenApiReference
                            {
                                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            builder.Services.AddScoped<LoginHandler>();

            builder.Services.AddScoped<GetCurrentUserHandler>();

            builder.Services.AddScoped<GetUsersHandler>();

            builder.Services.AddScoped<DeactivateUserHandler>();

            builder.Services.AddScoped<GetUserHandler>();

            builder.Services.AddScoped<GetPesronHandler>();

            builder.Services.AddScoped<AddUserHandler>();

            builder.Services.AddInfrastructure();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
                    
                    ValidateIssuer = true,
                    
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    
                    ValidateAudience = true,
                    
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    
                    ValidateLifetime = true,
                    
                    ClockSkew = TimeSpan.Zero
                };
            });

            builder.Services.AddAuthorization(options =>
            {
                foreach (string permission in Permissions.GetAll())
                {
                    options.AddPolicy(permission, policy =>
                    {
                        policy.Requirements.Add(new PermissionRequirement(permission));
                    });
                }
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseGlobalExceptionHandling();

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
