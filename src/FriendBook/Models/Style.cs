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

        public int UserId { get; set; }
        public User User { get; set; }

        public string PostBackgroundColor { get; set; }
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
            this.BackgroundColor = "#004080";
            this.WallBackgroundColor = "#0080c0";
            this.PostBackgroundColor = "#808080";
            this.FontColor = "#000000";
            this.FontSize = 14;
            this.FontFamily = "Times New Roman";
            this.NavColor = "#808080";
            this.DetailColor = "#c0c0c0";
        }
    }
}
