using System.ComponentModel.DataAnnotations.Schema;

namespace MyFriendsV3.Models
{
    [Table("Pictures")]
    public class Picture
    {
        public int Id { get; set; }
        public string PictureName { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public byte[] PictureFile { get; set; }
    }
}
