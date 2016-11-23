using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FriendBook.Models
{
    public class Style
    {
        [Key]
        public int StyleId { get; set; }
        [Required]
        public int UserId { get; set; }
        public User User { get; set; }

        public string BackgroundColor { get; set; }
        public string FontColor { get; set; }
        public int FontSize { get; set; }
        public string FontFamily { get; set; }
        public string NavColor { get; set; }
        public string DetailColor { get; set; }
    }
}
