using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using KarateClub.Application.Interfaces;
using KarateClub.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace KarateClub.Infrastructure.Authentication
{
    public class JwtCurrentUser : ICurrentUser
    {
        private readonly IHttpContextAccessor _context;

        public JwtCurrentUser(IHttpContextAccessor context)
        {
            _context = context;
        }

        private ClaimsPrincipal User => _context.HttpContext?.User ?? new ClaimsPrincipal();

        public bool IsAuthenticated => User.Identity?.IsAuthenticated ?? false;

        public int UserId
        {
            get
            {
                if (!IsAuthenticated)
                    return 0;

                return int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value);
            }
        }

        public string Username => User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)!.Value;

        public bool IsSuperAdmin => bool.Parse(User.Claims.FirstOrDefault(c => c.Type == "IsSuperAdmin")!.Value);

        public bool IsActive => bool.Parse(User.Claims.FirstOrDefault(c => c.Type == "IsActive")!.Value);

        public IReadOnlyCollection<string> permissions => User.Claims.Where(c => c.Type == "permission").Select(c => c.Value).ToList();
    }
}