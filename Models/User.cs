using System.ComponentModel.DataAnnotations.Schema;

namespace MyFriendsV3.Models
{
    [Table("Users")]
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public ICollection<Picture> UserPictures { get; set; } = new List<Picture>();
        public int? ProfilePictureId { get; set; }
        public Picture? ProfilePicture { get; set; }
    }
}
