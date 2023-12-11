using EfCore.context;
using EfCore.dto;
using EfCore.entity;
using Microsoft.EntityFrameworkCore;
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

        public void CreateRequest(RequestDTO requestDTO)
        {
            try
            {
                Request newRequest = new Request
                {
                    RequestName = requestDTO.RequestName,
                    Description = requestDTO.Description,
                    Location = requestDTO.Location,
                    CreatorUserRef = requestDTO.CreatorUserRef,
                    ContactPhone = requestDTO.ContactPhone,
                    ContactEmail = requestDTO.ContactEmail,
                    RoleRef = requestDTO.RoleRef,
                    CreatedAt = DateTime.Now,
                    RequestStatusId = 1,
                    Categories = GetRequestCategories((List<int>)requestDTO.Categories)
                };

                context.Requests.Add(newRequest);

                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error creating request: {ex.Message}");
            }
        }

        private List<RequestCategory> GetRequestCategories(List<int> categoryIds)
        {
            List<RequestCategory> requestCategories = new List<RequestCategory>();

            foreach (var categoryId in categoryIds)
            {
                RequestCategory category = context.RequestCategories.Find(categoryId);
                if (category != null)
                {
                    requestCategories.Add(category);
                }
            }

            return requestCategories;
        }


        public void UpdateRequest(Request currentRequest, string requestName, string description, string contactPhone,
            string contactEmail, string location, List<int> selectedCategoryIds)
        {
            try
            {
                currentRequest.RequestName = requestName;
                currentRequest.Description = description;
                currentRequest.ContactPhone = contactPhone;
                currentRequest.ContactEmail = contactEmail;
                currentRequest.Location = location;

                if (!context.Entry(currentRequest).Collection(r => r.Categories).IsLoaded)
                {
                    context.Entry(currentRequest).Collection(r => r.Categories).Load();
                }

                currentRequest.Categories.Clear();

                currentRequest.Categories = GetRequestCategories(selectedCategoryIds);

                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Помилка при оновленні даних запиту: {ex.Message}");
            }
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

        public RequestRole GetRequestRoleById(int id)
        {
            Request request = GetRequestById(id);
            if (request == null)
            {
                throw new ApplicationException("Запит з таким id: " + request + " не існує!");
            }
            return context.RequestRoles.Find(request.RoleRef);
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

        public List<Request> GetOrderRequestsByEmailAndRole(string userEmail)
        {
            return context.Requests
                .Where(r => r.CreatorUserRefNavigation.Email == userEmail && r.RoleRef == 1)
                .ToList();
        }

        public Request SearchRequestByName(string requestName)
        {
            return context.Requests
                .FirstOrDefault(r => EF.Functions.ILike(r.RequestName, $"%{requestName}%"));
        }

        public List<string> GetRequestCategoriesNames(int requestId)
        {
            try
            {
                Request request = GetRequestById(requestId);

                List<string> categoryNames = request.Categories.Select(c => c.CategoryName).ToList();

                if (categoryNames.Count == 0)
                {
                    categoryNames = context.Requests
                        .Where(r => r.RequestId == requestId)
                        .SelectMany(r => r.Categories.Select(c => c.CategoryName))
                        .ToList();
                }

                return categoryNames;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error getting request categories: {ex.Message}");
            }
        }

        public int GetActiveRequestsCount(int pageSize)
        {
            return context.Requests
                .Where(r => r.RequestStatusId == requestStatusService.GetActiveRequestStatus().StatusId)
                .Take(pageSize)
                .Count();
        }

        public List<Request> GetActiveRequests(int pageSize)
        {
            return context.Requests
                .Where(r => r.RequestStatusId == requestStatusService.GetActiveRequestStatus().StatusId)
                .Take(pageSize)
                .ToList();
        }

        public List<Request> GetMoreRequests(int currentPage, int pageSize)
        {
            /*try
            {
                int startIndex = (currentPage - 1) * pageSize;

                List<Request> moreRequests = context.Requests
                    .Where(r => r.RequestStatusId == requestStatusService.GetActiveRequestStatus().StatusId)
                    .OrderByDescending(r => r.CreatedAt) 
                    .Skip(startIndex)
                    .Take(pageSize)
                    .ToList();

                return moreRequests;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error getting more requests: {ex.Message}");
            }*/

            int skipCount = (currentPage - 1) * pageSize;
            return context.Requests
                .Where(r => r.RequestStatusId == requestStatusService.GetActiveRequestStatus().StatusId)
                .OrderByDescending(r => r.CreatedAt)
                .Skip(skipCount)
                .Take(pageSize)
                .ToList();
        }

    }
}
