using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;

namespace Domain.Interfaces.DbAbstractions.Entitys
{
    public interface IRoleManagerManipulators
    {
        IQueryable<ApplicationUser> GetAllUsersByRole(string roleId, string userId);
    }
}
