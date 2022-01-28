using System.Collections.Generic;
using System.Threading.Tasks;
using Miritush.DAL.Model;

namespace Miritush.Services.Abstract
{
    public interface IServicesService
    {
        Task<List<DTO.Service>> GetServicesAsync();
        Task<DTO.Service> CreateServiceAsync(string serviceName);
        Task<DTO.Service> GetServiceById(int id);
    }
}