using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using DevHouseTW.Models;
using Domain.DbContext;
using Domain.IdentityManagers;
using Domain.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace DevHouseTW.Controllers
{
    [Authorize]
    [RoutePrefix("api/Friendship")]
    public class FriendshipController : ApiController
    {
        ApplicationDbContext dbContext = new ApplicationDbContext();
        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }

        [HttpGet]
        [Route("AboutMe")]
        public async Task<IHttpActionResult> AboutMe()
        {
            var result = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            return Json(new
            {
                result.Id,
                result.Email,
                result.FirstName,
                result.LastName
            });
        }

        [HttpGet]
        [Route("AllUsers")]
        public async Task<IHttpActionResult> AllUsers()
        {
            var carentUser = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            var roleUsersId = dbContext.Roles.FirstOrDefault(e => e.Name == "user")?.Id;
            var users = dbContext.Users.Where(x => x.Roles.Select(y => y.RoleId).Contains(roleUsersId) &&
                                                   x.Id != carentUser.Id)
                .Select(e => new
                {
                    e.Id,
                    e.FirstName,
                    e.LastName
                });

            if (!users.Any())
            {
                return BadRequest("Пользователи не найдены");
            }

            return Json(users);
        }

        [HttpPost]
        [Route("FriendsUserById")]
        public async Task<IHttpActionResult> FriendsUserById(FriendshipIdModel model)
        {

            var user = await UserManager.FindByIdAsync(model.Id);

            if (user == null || !user.Friendships.Any())
            {
                return BadRequest("Пользователь или его друзья не найдены");
            }

            List<FrienResult> result = new List<FrienResult>();

            foreach (var userFriendship in user.Friendships)
            {
                var friend = await UserManager.FindByIdAsync(userFriendship.FriendId);

                result.Add(new FrienResult()
                {
                    FriendId = userFriendship.FriendId,
                    FirstName = friend.FirstName,
                    LastName = friend.LastName,
                    FriendshipType = userFriendship.FriendshipType,
                    FriendshipDuration = userFriendship.FriendshipDuration
                });
            }

            return Json(result);

        }

        [HttpPost]
        [Route("AddFriend")]
        public async Task<IHttpActionResult> AddFriend(FriendshipAddFriendModel model)
        {
            var carentUser = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            if (carentUser.Friendships.Any(e => e.FriendId == model.Id))
            {
                return BadRequest("Данный пользователь уже добавлен в друзья");
            }

            var friend = await UserManager.FindByIdAsync(model.Id);

            if (friend == null)
            {
                return BadRequest("Добавляемый пользователь не найден");
            }

            dbContext.Friendships.Add(new Friendship()
            {
                UserId = carentUser.Id,
                FriendId = friend.Id,
                FriendshipType = model.FriendshipType,
                FriendshipDuration = DateTime.Now
            });

            await dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        [Route("DeleteFriend")]
        public async Task<IHttpActionResult> DeleteFriend(FriendshipIdModel model)
        {
            var carentUser = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            var friend = carentUser.Friendships.FirstOrDefault(e => e.FriendId == model.Id);

            if (friend == null)
            {
                return BadRequest("Данный пользователь отутвует в списке друзей");
            }

            dbContext.Friendships.Remove(dbContext.Friendships.FirstOrDefault(e => e.FriendId == model.Id &&
                                                                                 e.UserId == carentUser.Id));
            await dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}
