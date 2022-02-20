using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Miritush.API.Extensions
{
    internal static class IdentityExtensions
    {
        internal static async Task<ClaimsPrincipal> AttachIdentityToContext(this HttpContext context, ClaimsPrincipal principal)
        {
            if (!principal.Identity.IsAuthenticated)
                throw new System.Exception(); //[GG] change to relevent exeption

            var username = principal.Identity.Name;
            if (username == null) return principal;

            var dbcontext = context.RequestServices.GetRequiredService<DAL.Model.booksDbContext>();
            var mapper = context.RequestServices.GetRequiredService<AutoMapper.IMapper>();

            var user = await dbcontext.Users
               .Where(user => user.UserName == username)
               .FirstOrDefaultAsync();

            if (user == null)
            {
                var user2 = await dbcontext.Customers
                    .Where(customer=>customer.PhoneNumber == username)
                    .FirstOrDefaultAsync();
            }

            var identity = mapper.Map<DTO.BookIdentity>(user);
            identity.AddClaims(((ClaimsIdentity)principal.Identity).Claims);

            return new ClaimsPrincipal(identity);
        }
    }
}
