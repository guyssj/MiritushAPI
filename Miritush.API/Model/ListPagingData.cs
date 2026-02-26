using System.ComponentModel.DataAnnotations;

namespace Miritush.API.Model
{
    public class ListPagingData
    {
        [Range(1, int.MaxValue)]
        public int PageNummber { get; set; } = 1;

        [Range(1, 100)]
        public int PageSize { get; set; } = 20;
    }
}