using System.Collections.Generic;
using System.Threading.Tasks;
using Miritush.DTO;

namespace Miritush.Services.Abstract
{
    public interface IServiceTypeService
    {
        Task CreateServiceType(
            string serviceTypeName,
            int serviceId,
            int duration,
            decimal price,
            string description = "");
        Task<ServiceType> Get(int id);
        Task<List<ServiceType>> GetByService(int serviceId);
        Task<List<ServiceType>> List();
    }
}