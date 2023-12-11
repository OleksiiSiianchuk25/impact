using EfCore.dto;
using EfCore.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EfCore.service
{
    internal interface IRequestService
    {
        List<Request> GetAllRequests();

        Request GetRequestById(int requestId);

        void CreateRequest(RequestDTO newRequest);

        void UpdateRequest(Request currentRequest, string requestName, string description, string contactPhone,
            string contactEmail, string location, List<int> selectedCategoryIds);

        void DeleteRequest(int requestId);

        List<Request> GetOrders();

        List<Request> GetPropositions();

        List<Request> GetActiveRequests();

        List<Request> GetInactiveRequests();
    }
}
