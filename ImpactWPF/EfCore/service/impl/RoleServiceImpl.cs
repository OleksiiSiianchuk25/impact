using EfCore.context;
using EfCore.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EfCore.service.impl
{
    public class RoleServiceImpl : IRoleService
    {
        private readonly ImpactDbContext context;

        public RoleServiceImpl()
        {
        }

        public RoleServiceImpl(ImpactDbContext context)
        {
            this.context = context;
        }

        public virtual Role GetOrdererRole()
        {
            return context.Roles.Find(1);
        }

        public virtual Role GetVolunteerRole()
        {
            return context.Roles.Find(2);
        }

        public virtual Role GetAdminRole()
        {
            return context.Roles.Find(3);
        }
    }
}
