using System;

namespace DevHouseTW.Models
{
    public class FriendshipAddFriendModel
    {
        public string Id { get; set; }
        public string FriendshipType { get; set; }
    }
    public class FriendshipIdModel
    {
        public string Id { get; set; }
    }

    public class FrienResult
    {
        public string FriendId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FriendshipType { get; set; }
        public DateTime FriendshipDuration { get; set; }
    }
}