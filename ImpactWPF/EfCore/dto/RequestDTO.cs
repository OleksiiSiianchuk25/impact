using EfCore.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EfCore.dto
{
    public class RequestDTO
    {
        public string RequestName { get; set; }
        public string Description { get; set; }
        public string ContactPhone { get; set; } 
        public string ContactEmail { get; set; }
        public string Location { get; set; }    
        public virtual ICollection<RequestCategory> Categories { get; set; } = new List<RequestCategory>();

    }
}
