using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Miritush.DAL.Model
{
    public partial class CustomerTimeline
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int Type { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public string Description { get; set; }
        public string Notes { get; set; }

        public virtual Customer Customer { get; set; }


    }
}