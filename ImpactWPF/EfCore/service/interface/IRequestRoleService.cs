using EfCore.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EfCore.service
{
    internal interface IRequestRoleService
    {
        RequestRole GetOrderRequestRole();

        RequestRole GetPropositionRequestRole();
    }
}
