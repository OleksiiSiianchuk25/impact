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
        public int? CreatorUserRef { get; set; }
        public int RoleRef { get; set; }
        public virtual ICollection<int> Categories { get; set; } = new List<int>();

        public RequestDTO()
        {
        }

        public RequestDTO(string requestName, string description, string contactPhone,
            string contactEmail, string location, int? creatorUserRef, 
            int roleRef, ICollection<int> categories)
        {
            RequestName = requestName;
            Description = description;
            ContactPhone = contactPhone;
            ContactEmail = contactEmail;
            Location = location;
            CreatorUserRef = creatorUserRef;
            RoleRef = roleRef;
            Categories = categories;
        }
    }
}
