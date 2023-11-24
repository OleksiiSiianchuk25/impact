using EfCore.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EfCore.service
{
    internal interface IRequestCategoryService
    {
        List<RequestCategory> GetAllCategories();

        RequestCategory GetCategoryById(int categoryId);
    }
}
