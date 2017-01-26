using System.Data.Entity;
using Domain.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Domain.DbContext
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Friendship> Friendships { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new FriendshipConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
