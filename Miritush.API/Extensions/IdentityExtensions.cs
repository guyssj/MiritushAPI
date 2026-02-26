using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Miritush.Helpers.Exceptions;
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
                throw new UnauthorizedException();

            var username = principal.Identity.Name;
            if (username == null) return principal;

            var dbcontext = context.RequestServices.GetRequiredService<DAL.Model.booksDbContext>();
            var mapper = context.RequestServices.GetRequiredService<AutoMapper.IMapper>();

            var user = await dbcontext.Users
               .AsNoTracking()
               .Where(user => user.UserName == username)
               .FirstOrDefaultAsync();

            DAL.Model.Customer customer = null;
            if (user == null)
            {
                customer = await dbcontext.Customers
                    .AsNoTracking()
                    .Where(customer => customer.PhoneNumber == username)
                    .FirstOrDefaultAsync();
                if (customer == null)
                    throw new UnauthorizedException();
            }


            var identity = mapper.Map<DTO.BookIdentity>(user == null ? customer : user);
            identity.AddClaims(((ClaimsIdentity)principal.Identity).Claims);

            return new ClaimsPrincipal(identity);
        }
    }
}
