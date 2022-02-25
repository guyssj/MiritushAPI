using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Miritush.DTO;

namespace Miritush.Services.Abstract
{
    public interface IUserContextService
    {
        BookIdentity Identity { get; }
    }
}