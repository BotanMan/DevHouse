using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.DbContext;
using Domain.Interfaces.DbAbstractions;
using Domain.Models;

namespace Domain.DomainRepositories
{
    public class ContextEFDbConnector : IApplicationEFDbConnector
    {
        private ApplicationDbContext dbContext;

        public ContextEFDbConnector()
        {
            dbContext = new ApplicationDbContext();
        }
        public IQueryable<ApplicationUser> GetAllUsersByRole(string roleId, string userId)
        {
            var roleUsersId = dbContext.Roles.FirstOrDefault(e => e.Name == "user")?.Id;
            var users = dbContext.Users.Where(x => x.Roles.Select(y => y.RoleId).Contains(roleUsersId) &&
                                                 x.Id != userId);
            return users;
        }

        public async Task<int> AddFriendAsync(Friendship friendship)
        {
            dbContext.Friendships.Add(new Friendship()
            {
                UserId = friendship.UserId,
                FriendId = friendship.FriendId,
                FriendshipType = friendship.FriendshipType,
                FriendshipDuration = friendship.FriendshipDuration
            });

           return await dbContext.SaveChangesAsync();
        }

        public async Task<int> RemoveFriendAsync(string carentUserId, string removingFriendId)
        {
            dbContext.Friendships.Remove(dbContext.Friendships.FirstOrDefault(e => e.FriendId == removingFriendId &&
                                                                      e.UserId == carentUserId));
            return await dbContext.SaveChangesAsync();
        }
    }
}
