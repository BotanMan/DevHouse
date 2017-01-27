using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;

namespace Domain.Interfaces.ModelAbstractions
{
    public interface IFriendshipManager
    {
        Task<int> AddFriendAsync(Friendship friendship);
        Task<int> RemoveFriendAsync(string carentUserId, string removingFriendId);
    }
}
