using KarateClub.Application.Interfaces.Repositories;
using KarateClub.Application.Interfaces;
using KarateClub.Infrastructure.Authentication;
using KarateClub.Infrastructure.Security;
using Microsoft.Extensions.DependencyInjection;
using KarateClub.Infrastructure.Persistence.Repositories;
using KarateClub.Infrastructure.Persistence.Connection;
using KarateClub.Infrastructure.Persistence.Data;
using KarateClub.Infrastructure.Authorization;
using Microsoft.AspNetCore.Authorization;

namespace KarateClub.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<IPersonRepository, PersonRepository>();

            services.AddScoped<IEncryptionService, BCryptEncryptionService>();

            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

            services.AddScoped<ICurrentUser, CurrentUserAccessor>();

            services.AddScoped<IDbConnectionFactory, SqlConnectionFactory>();

            services.AddScoped<IAuthorizationHandler, ActiveUserAuthorizationHandler>();

            services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

            services.AddScoped<IBeltRankRepository, BeltRankRepository>();

            services.AddScoped<ISubscriptionPlanRepository, SubscriptionPlanRepository>();

            services.AddScoped<IInstructorRepository, InstructorRepository>();

            return services;
        }
    }
}