using System;
using System.Collections;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using DevHouseTW.Models;
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
            //try
            //{
            var result = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            return Json(new
            {
                result.Id,
                result.Email,
                result.FirstName,
                result.LastName
            });
            //}
            //catch (Exception exception)
            //{
            //    return BadRequest();
            //}

        }

        [HttpGet]
        [Route("AllUsers")]
        public async Task<IHttpActionResult> AllUsers()
        {
            //try
            //{
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
            //}
            //catch (Exception exception)
            //{
            //    return BadRequest();
            //}

        }

        [HttpPost]
        [Route("FriendsUserById")]
        public async Task<IHttpActionResult> FriendsUserById(FriendshipIdModel model)
        {
            //try
            //{
            var user = await UserManager.FindByIdAsync(model.Id);
            //var data = dbContext.Friendships.Select(e => e.UserId == model.Id);

            if (user == null)
            {
                return NotFound();
            }

            var frends = user.Friendships.Select(e => new
            {
                e.UserId,
                e.FriendshipType,
                e.FriendshipDuration,
                UserFNLN = dbContext.Users.Where(el => el.Id == e.UserId).Select(q => new { q.FirstName, q.LastName })
            });

            //var enumerable = frends as IList ?? frends.ToList();

            //if (enumerable.Count > 0)
            //{
            //    return NotFound();
            //}

            return Json(frends);
            //}
            //catch (Exception exception)
            //{
            //    return BadRequest();
            //}
        }

        [HttpPost]
        [Route("AddFriend")]
        public async Task<IHttpActionResult> AddFriend(FriendshipAddFriendModel model)
        {
            var carentUser = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            var friend = await UserManager.FindByIdAsync(model.Id);

            if (carentUser == null || friend == null)
            {
                return NotFound();
            }
            
            dbContext.Friendships.Add(new Friendship()
            {
                UserId = carentUser.Id,
                FriendId = friend.Id,
                FriendshipType = model.FriendshipType,
                FriendshipDuration = DateTime.Now
            });

            dbContext.SaveChanges();

            return Ok();
        }

        [HttpPost]
        [Route("DeleteFriend")]
        public async Task<IHttpActionResult> DeleteFriend(FriendshipIdModel model)
        {
            var carentUser = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            var friend = await UserManager.FindByIdAsync(model.Id);

            if (carentUser == null || friend == null)
            {
                return NotFound();
            }

            dbContext.Friendships
                        .Remove(
                            dbContext
                                .Friendships
                                .Single(e => e.UserId == carentUser.Id &&
                                             e.FriendId == friend.Id));

            dbContext.SaveChanges();

            return Ok();
        }
    }
}
