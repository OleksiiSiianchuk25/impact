using EfCore.context;
using EfCore.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EfCore.service.impl
{
    public class RequestCategoryServiceImpl : IRequestCategoryService
    {
        private readonly ImpactDbContext context;

        public RequestCategoryServiceImpl(ImpactDbContext context)
        {
            this.context = context;
        }

        public List<RequestCategory> GetAllCategories()
        {
            return context.RequestCategories.ToList();
        }

        public RequestCategory GetCategoryById(int categoryId)
        {
            RequestCategory requestCategory = context.RequestCategories.Find(categoryId);
            if (requestCategory == null)
            {
                throw new ApplicationException("Request category with id: " + categoryId + " does not exist!");
            }
            return requestCategory;
        }
    }
}
