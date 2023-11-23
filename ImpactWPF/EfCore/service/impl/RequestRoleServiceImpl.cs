using EfCore.context;
using EfCore.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EfCore.service.impl
{
    public class RequestRoleServiceImpl : IRequestRoleService
    {
        private readonly ImpactDbContext context;

        public RequestRoleServiceImpl(ImpactDbContext context)
        {
            this.context = context;
        }

        public RequestRole GetOrderRequestRole()
        {
            return context.RequestRoles.Find(1L);
        }

        public RequestRole GetPropositionRequestRole()
        {
            return context.RequestRoles.Find(2L);
        }
    }
}
