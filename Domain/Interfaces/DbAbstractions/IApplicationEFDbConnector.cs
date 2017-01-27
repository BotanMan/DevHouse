using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces.DbAbstractions.Entitys;
using Domain.Interfaces.ModelAbstractions;

namespace Domain.Interfaces.DbAbstractions
{
    public interface IApplicationEFDbConnector : IRoleManagerManipulators, IFriendshipManager
    {
    }
}
