using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using KarateClub.Application.Interfaces;
using KarateClub.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace KarateClub.Infrastructure.Authentication
{
    public sealed class CurrentUserAccessor : ICurrentUser
    {
        private readonly IHttpContextAccessor _accessor;

        public CurrentUserAccessor(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public User User => (User)_accessor.HttpContext!.Items["CurrentUser"]!;

        public int UserId => User.Id;

        public string Username => User.Username;

        public bool IsSuperAdmin => User.IsSuperAdmin;

        public bool IsActive => User.IsActive;

        public IReadOnlyCollection<string> Permissions => User.Permissions.Select(p => p.Code).ToList();
    }
}