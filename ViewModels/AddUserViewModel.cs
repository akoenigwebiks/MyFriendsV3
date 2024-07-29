using MyFriendsV3.Models;

namespace MyFriendsV3.ViewModels
{
    public class AddUserViewModel
    {
        public User User { get; set; }
        public IFormFile ProfileImage { get; set; }
    }
}
