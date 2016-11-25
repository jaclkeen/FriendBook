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

        public string WallBackgroundColor { get; set; }
        public string BackgroundColor { get; set; }
        public string FontColor { get; set; }
        public int FontSize { get; set; }
        public string FontFamily { get; set; }
        public string NavColor { get; set; }
        public string DetailColor { get; set; }

        public Style() { }

        public Style(int id){
            this.UserId = id;
            this.BackgroundColor = "lightgrey";
            this.WallBackgroundColor = "#6495ED";
            this.FontColor = "black";
            this.FontSize = 16;
            this.FontFamily = "Times New Roman";
            this.NavColor = "darkgrey";
            this.DetailColor = "lightblue";
        }
    }
}
