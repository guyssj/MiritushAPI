using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Miritush.DTO
{
    public class BookIdentity : ClaimsIdentity
    {
        public override string AuthenticationType => "BookNail";
        public override bool IsAuthenticated => true;
        public override string Name => UserName;
        public string UserName => FindFirst(NameClaimType)?.Value;
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int UserId { get; set; }
        public string RoleName
        {
            get => FindFirst(RoleClaimType)?.Value;
            set
            {
                foreach (var claim in FindAll(RoleClaimType))
                    TryRemoveClaim(claim);
                AddClaim(new System.Security.Claims.Claim(RoleClaimType, value));
            }
        }

    }
}
