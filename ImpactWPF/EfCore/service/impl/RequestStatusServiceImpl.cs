using EfCore.context;
using EfCore.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EfCore.service.impl
{
    public class RequestStatusServiceImpl : IRequestStatusService
    {
        private readonly ImpactDbContext context;

        public RequestStatusServiceImpl(ImpactDbContext context)
        {
            this.context = context;
        }

        public RequestStatus GetActiveRequestStatus()
        {
            return context.RequestStatuses.Find(1);
        }

        public RequestStatus GetInactiveRequestStatus()
        {
            return context.RequestStatuses.Find(2);
        }
    }
}
