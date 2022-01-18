using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Miritush.Services.Abstract
{
    public interface ICustomerService
    {
        Task<List<Miritush.DAL.Model.Customer>> GetCustomersAsync();
    }
}