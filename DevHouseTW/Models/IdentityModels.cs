using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace DevHouseTW.Models
{
    // Чтобы добавить данные профиля для пользователя, можно добавить дополнительные свойства в класс ApplicationUser. Дополнительные сведения см. по адресу: http://go.microsoft.com/fwlink/?LinkID=317594.
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public ICollection<Friendship> Friendships { get; set; }

        public ApplicationUser()
        {
            Friendships = new List<Friendship>();
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Обратите внимание, что authenticationType должен совпадать с типом, определенным в CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Здесь добавьте настраиваемые утверждения пользователя
            return userIdentity;
        }
    }

    public class Friendship
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime FriendshipDuration { get; set; }
        public string FriendshipType { get; set; }
    }

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

        class FriendshipConfiguration : EntityTypeConfiguration<Friendship>
        {
            public FriendshipConfiguration()
            {
                HasKey(e => e.Id);
                Property(e => e.UserId).IsRequired();
                Property(e => e.FriendshipDuration).IsRequired();
                Property(e => e.FriendshipType).IsRequired();
            }
        }
    }
}