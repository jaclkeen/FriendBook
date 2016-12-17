using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FriendBook.Models
{
    public class Relationship
    {
        [Key]
        public int RelationshipId { get; set; }

        [Required]
        public int SenderUserId { get; set; }
        public User SenderUser { get; set; }

        [Required]
        public int ReciverUserId { get; set; }
        public User ReceivingUser { get; set;}

        [Required]
        public int Status { get; set; }

        //Status Int Meanings:
        //    0: Pending
        //    1: Accepted
        //    2: Declined
        //    3: Blocked
    }
}
