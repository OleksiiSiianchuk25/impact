using EfCore.context;
using EfCore.dto;
using EfCore.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EfCore.service.impl
{
    public class RequestServiceImpl : IRequestService
    {
        private readonly ImpactDbContext context;
        private readonly RequestRoleServiceImpl requestRoleService;
        private readonly RequestStatusServiceImpl requestStatusService;

        public RequestServiceImpl(ImpactDbContext context)
        {
            this.context = context;
            this.requestRoleService = new RequestRoleServiceImpl(context);
            this.requestStatusService = new RequestStatusServiceImpl(context);
        }

        public void CreateRequest(RequestDTO newRequest)
        {
            throw new NotImplementedException();
        }

        public void UpdateRequest(Request request)
        {
            throw new NotImplementedException();
        }

        public void DeleteRequest(int requestId)
        {
            Request request = GetRequestById(requestId);
            context.Requests.Remove(request);
            context.SaveChanges();
        }

        public Request GetRequestById(int requestId)
        {
            Request request = context.Requests.Find(requestId);
            if (request == null)
            {
                throw new ApplicationException("Request with id: " + request + " does not exist!");
            }
            return request;
        }

        public List<Request> GetActiveRequests()
        {
            return context.Requests
                .Where(r => r.RequestStatusId == requestStatusService.GetActiveRequestStatus().StatusId)
                .ToList();
        }

        public List<Request> GetAllRequests()
        {
            return context.Requests.ToList();
        }

        public List<Request> GetInactiveRequests()
        {
            return context.Requests
                .Where(r => r.RequestStatusId == requestStatusService.GetInactiveRequestStatus().StatusId)
                .ToList();
        }

        public List<Request> GetOrders()
        {
            return context.Requests
                .Where(r => r.RoleRefNavigation.RoleName == requestRoleService.GetOrderRequestRole().RoleName)
                .ToList();
        }

        public List<Request> GetPropositions()
        {
            return context.Requests
                .Where(r => r.RoleRefNavigation.RoleName == requestRoleService.GetPropositionRequestRole().RoleName)
                .ToList();
        }
    }
}
