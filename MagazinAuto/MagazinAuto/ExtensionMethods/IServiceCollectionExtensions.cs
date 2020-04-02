using MagazinAuto.Controllers;
using MagazinAuto.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Security.Claims;

namespace MagazinAuto.ExtensionMethods
{
    public static class IServiceCollectionExtensions
    {
        public static void AddCurrentUser(this IServiceCollection services)
        {
            _ = services.AddScoped(serviceProvider =>
            {
                var httpContextAccessor = serviceProvider.GetService<IHttpContextAccessor>();
                var services = serviceProvider.GetService<Services>();
                var email = httpContextAccessor.HttpContext.User
                                    .FindFirst(ClaimTypes.Email)?
                                    .Value;

                var user = services.GetUser(email);

                return user == null ? new User() : new User
                {
                    Email = user.Email,
                    Id = user.Id,
                    Nume = user.Nume,
                    Telefon = user.Telefon,
                    IsAuthenticated = true
                };
            });
        }
    }
}
