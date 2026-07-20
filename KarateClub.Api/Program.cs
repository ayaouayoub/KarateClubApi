
using KarateClub.Application.Handlers.User;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using KarateClub.Infrastructure;
using KarateClub.Application.Security;
using KarateClub.Api.Middlewares;
using KarateClub.Infrastructure.Authorization;
using KarateClub.Application.Handlers.Person;
using Microsoft.AspNetCore.Authorization;
using Serilog;
using KarateClub.Application.Handlers.BeltRank;
using KarateClub.Application.Handlers.SubscriptionPlan;
using KarateClub.Application.Handlers.Instructor;
using KarateClub.Application.Handlers.Member;
using KarateClub.Application.Handlers.Member.Commands;

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

            builder.Services.AddScoped<GetUserByPersonIdHandler>();

            builder.Services.AddScoped<UpdateUserHandler>();

            builder.Services.AddScoped<ChangeMyUsernameHandler>();

            builder.Services.AddScoped<ChangeMyPasswordHandler>();

            builder.Services.AddScoped<GetPeopleHandler>();

            builder.Services.AddScoped<AddNewPersonHandler>();

            builder.Services.AddScoped<UpdatePersonHandler>();

            builder.Services.AddScoped<GetBeltRankHandler>();

            builder.Services.AddScoped<GetBeltsHandler>();

            builder.Services.AddScoped<ChangeBeltTestFeesHandler>();

            builder.Services.AddScoped<GetSubscriptionPlanHandler>();

            builder.Services.AddScoped<GetSubscriptionPlansHandler>();

            builder.Services.AddScoped<CreateSubscriptionPlanHandler>();

            builder.Services.AddScoped<DeactivatePlanHandler>();

            builder.Services.AddScoped<GetInstructorHandler>();

            builder.Services.AddScoped<GetInstructorByPersonIdHandler>();

            builder.Services.AddScoped<GetInstructorsHandler>();

            builder.Services.AddScoped<DeactivateInstructorHandler>();

            builder.Services.AddScoped<ActivateInstructorHandler>();

            builder.Services.AddScoped<UpdateCurrentBletRankHandler>();

            builder.Services.AddScoped<CreateInstructorHandler>();

            builder.Services.AddScoped<GetMemberHandler>();

            builder.Services.AddScoped<GetMemberByPersonIdHandler>();

            builder.Services.AddScoped<GetMembersHandler>();

            builder.Services.AddScoped<DeactivateMemberHandler>();

            builder.Services.AddScoped<ActivateMemberHandler>();

            builder.Services.AddScoped<UpdateMemberCurrentBletRankHandler>();

            builder.Services.AddScoped<UpdateEmergencyContactInfoHandler>();

            builder.Services.AddScoped<GetInstructorMembersHandler>();

            builder.Services.AddScoped<GetMemberInstructorsHandler>();

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
                options.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .AddRequirements(new ActiveUserRequirement())
                    .Build();

                foreach (string permission in Permissions.GetAll())
                {
                    options.AddPolicy(permission, policy =>
                    {
                        policy.RequireAuthenticatedUser();

                        policy.AddRequirements(
                            new ActiveUserRequirement(),
                            new PermissionRequirement(permission));
                    });
                }
            });

            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger();

            builder.Host.UseSerilog();

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
