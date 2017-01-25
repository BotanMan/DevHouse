using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DevHouseTW.Models
{
    public class Friendship
    {
        [Key]
        public int Id { get; set; }
        public string FriendId { get; set; }
        public DateTime FriendshipDuration { get; set; }
        public string FriendshipType { get; set; }
        
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }
    }

    class FriendshipConfiguration : EntityTypeConfiguration<Friendship>
    {
        public FriendshipConfiguration()
        {
            HasKey(e => e.Id);
            Property(e => e.UserId).IsRequired();
            Property(e => e.FriendId).IsRequired();
            Property(e => e.FriendshipDuration).IsRequired();
            Property(e => e.FriendshipType).IsRequired();
        }
    }

    public enum FriendshipType
    {
        BestFriend = 1,
        LoveOfLife,
        Relative,
        Friend,
        Colleague
    }
}
