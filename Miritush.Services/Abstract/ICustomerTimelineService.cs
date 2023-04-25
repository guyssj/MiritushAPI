using System.Threading.Tasks;

namespace Miritush.Services.Abstract
{
    public interface ICustomerTimelineService
    {
        Task SaveTimeLine(
            int customerId,
            DTO.Enums.TimelineType type,
            string description = null,
            string notes = null);
    }
}