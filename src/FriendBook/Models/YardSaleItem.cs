using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace FriendBook.Models
{
    public class YardSaleItem
    {
        [Key]
        public int YardSaleItemId { get; set; }

        [Required]
        public int PostingUserId { get; set; }
        public User PostingUser { get; set; }

        [Required]
        public string ItemName { get; set; }

        [Required]
        public string ItemDescription { get; set; }

        [Required]
        public double ItemPrice { get; set; }

        [Required]
        public DateTime DatePosted { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public string ItemImage1 { get; set; }

        public string ItemImage2 { get; set; }

        public string ItemImage3 { get; set; }

        public string ItemImage4 { get; set; }

        public ICollection<Comment> ItemComments { get; set; }
    }

}
