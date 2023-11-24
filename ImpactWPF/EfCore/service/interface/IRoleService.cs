using EfCore.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EfCore.service
{
    internal interface IRoleService
    {
        Role GetOrdererRole();

        Role GetVolunteerRole();

        Role GetAdminRole();
    }
}
