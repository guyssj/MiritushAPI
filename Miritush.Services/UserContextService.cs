using System.Linq;
using Microsoft.AspNetCore.Http;
using Miritush.Services.Abstract;

namespace Miritush.Services
{
    public class UserContextService : IUserContextService
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public UserContextService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public DTO.BookIdentity Identity =>
            httpContextAccessor.HttpContext.User.Identities
                .OfType<DTO.BookIdentity>()
                .FirstOrDefault();

    }
}