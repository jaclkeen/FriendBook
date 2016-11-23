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
        public int UserId1 { get; set; }

        [Required]
        public int UserId2 { get; set; }

        [Required]
        public int Status { get; set; }

        //Status Int Meanings:
        //    0: Pending
        //    1: Accepted
        //    2: Declined
        //    3: Blocked
    }
}
